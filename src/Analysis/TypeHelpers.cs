using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine.Analysis
{
    internal static class TypeHelpers
    {
        public static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().BaseType == typeof(Enum);
        }

        internal static bool IsList(Type type)
        {
            return type == typeof(List<string>);
        }

        public static void ScanTypeForProperties<TOptions>(out TypeArgumentInfo tInfo)
        {
            tInfo = new TypeArgumentInfo();
            PropertyInfo[] propertiesOnType = typeof(TOptions).GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            // first of all, find the commandArgument, if any.
            tInfo.ActionArgument = FindCommandProperty(propertiesOnType);

            // we want to be able to support empty groups.
            if (tInfo.ActionArgument != null && IsEnum(tInfo.ActionArgument.PropertyType))
            {
                // get the values of the enum.
                var enumValues = Enum.GetValues(tInfo.ActionArgument.PropertyType);

                // Sort the enum by the enum values
                Array.Sort(enumValues);

                // add them to the dictionary now to make sure we preserve the order
                foreach (var val in enumValues)
                {
                    if (!tInfo.ArgumentGroups.ContainsKey(val.ToString()))
                    {
                        tInfo.ArgumentGroups.Add(val.ToString(), new ArgumentGroupInfo());
                    }
                }
            }

            // parse the rest of the properties
            foreach (PropertyInfo property in propertiesOnType)
            {
                // get the group containing this property (note: more than one group can have the same property)
                // this allows common required parameters

                List<ArgumentGroupInfo> groupsWhereThePropertyIsParticipating = GetGroupsForProperty(tInfo, property);
                List<ActualArgumentAttribute> actualAttribs = property.GetCustomAttributes<ActualArgumentAttribute>().ToList();

                if (actualAttribs.Count > 1)
                {
                    throw new ArgumentException($"Only one of Required/Optional attribute are allowed per property ({property.Name}). Help information might be incorrect!");
                }

                // if we have no attributes on that property, move on
                ActualArgumentAttribute baseAttrib = actualAttribs.FirstOrDefault();
                if (baseAttrib == null)
                {
                    continue;
                }

                // add the property to the groups it is a part of
                if (baseAttrib is RequiredArgumentAttribute)
                {
                    foreach (ArgumentGroupInfo grpPropInfo in groupsWhereThePropertyIsParticipating)
                    {
                        // do we have an override for this property? If we do, use that, otherwise use the regular one.
                        if (!grpPropInfo.OverridePositions.TryGetValue(property, out int requiredPositionIndex))
                        {
                            requiredPositionIndex = (int)baseAttrib.GetArgumentId();
                        }

                        if (grpPropInfo.RequiredArguments.ContainsKey(requiredPositionIndex))
                        {
                            throw new ArgumentException("Two required arguments share the same position!!");
                        }

                        // if we have a collection argument, keep track of it
                        if (baseAttrib.IsCollection)
                        {
                            // if we already have defined a required collection argument, throw an error
                            if (grpPropInfo.IndexOfCollectionArgument >= 0)
                            {
                                throw new ArgumentException("Cannot declare two required collection arguments. Please convert one of them to an optional collection one.");
                            }
                            grpPropInfo.IndexOfCollectionArgument = requiredPositionIndex;
                        }

                        grpPropInfo.RequiredArguments[requiredPositionIndex] = property;
                    }
                }
                else if (baseAttrib is OptionalArgumentAttribute)
                {
                    foreach (ArgumentGroupInfo grpPropInfo in groupsWhereThePropertyIsParticipating)
                    {
                        if (grpPropInfo.OptionalArguments.ContainsKey((string)baseAttrib.GetArgumentId()))
                        {
                            throw new ArgumentException("Two optional arguments share the same name!!");
                        }

                        grpPropInfo.OptionalArguments[(string)baseAttrib.GetArgumentId()] = property;
                    }
                }
            }

            // remove the empty one, if empty
            if (tInfo.ArgumentGroups.TryGetValue(string.Empty, out ArgumentGroupInfo grp))
            {
                if (grp.OptionalArguments.Count == 0 && grp.RequiredArguments.Count == 0)
                    tInfo.ArgumentGroups.Remove(string.Empty);
            }

            // validate that the collection is the last argument
            foreach (KeyValuePair<string, ArgumentGroupInfo> group in tInfo.ArgumentGroups)
            {
                if (group.Value.IndexOfCollectionArgument >= 0 && group.Value.IndexOfCollectionArgument != group.Value.RequiredArguments.Count - 1)
                {
                    if (!string.IsNullOrEmpty(group.Key))
                    {
                        throw new ArgumentException($"The required collection argument for group `{group.Key}` needs to be on the last position of the required arguments.");
                    }
                    else
                    {
                        throw new ArgumentException("The required collection argument needs to be on the last position of the required arguments.");
                    }
                }
            }
        }

        private static List<ArgumentGroupInfo> GetGroupsForProperty(TypeArgumentInfo tInfo, PropertyInfo property)
        {
            List<ArgumentGroupInfo> groupsForThisProperty = new List<ArgumentGroupInfo>();

            var customAttributes = property.GetCustomAttributes<GroupAttribute>();

            if (!customAttributes.Any())
            {
                // we need to make sure that we don't add this here is the attribute we are adding is the Action one (which is outside of groups)
                if (!property.GetCustomAttributes<ActionArgumentAttribute>().Any())
                {
                    // we have the simple case where we don't have groups defined
                    groupsForThisProperty.Add(GetArgumentInfoForSingleGroup(string.Empty, tInfo));
                }
            }
            else
            {
                // we have the complex scenario where groups are present

                // there are 2 types of arguments in a group:
                //  - common ones, flagged with the CommonArgumentAttribute
                //  - specific to each command group, flagged with ArgumentGroupAttribute
                groupsForThisProperty.AddRange(GetArgumentInfoForGroups(tInfo, customAttributes, property));
            }

            return groupsForThisProperty;
        }

        private static IEnumerable<ArgumentGroupInfo> GetArgumentInfoForGroups(TypeArgumentInfo tInfo, IEnumerable<GroupAttribute> customAttributes, PropertyInfo property)
        {
            foreach (var commandGroup in customAttributes)
            {
                if (commandGroup is ArgumentGroupAttribute)
                {
                    var argGroupAttribute = commandGroup as ArgumentGroupAttribute;
                    ArgumentGroupInfo grpPropInfo;
                    if (!tInfo.ArgumentGroups.TryGetValue(argGroupAttribute.Name, out grpPropInfo))
                    {
                        grpPropInfo = new ArgumentGroupInfo();
                        tInfo.ArgumentGroups[argGroupAttribute.Name] = grpPropInfo;
                    }

                    // the list of groups is created eagerly if we have an action that is an enum.
                    if (argGroupAttribute.OverrideRequiredPosition >= 0)
                    {
                        grpPropInfo.OverridePositions[property] = argGroupAttribute.OverrideRequiredPosition;
                    }

                    yield return grpPropInfo;
                }
                else if (commandGroup is CommonArgumentAttribute)
                {
                    // return all the groups since this is a commond argument
                    foreach (var item in tInfo.ArgumentGroups.Values)
                    {
                        yield return item;
                    }
                }
            }
        }

        private static ArgumentGroupInfo GetArgumentInfoForSingleGroup(string groupName, TypeArgumentInfo tInfo)
        {
            ArgumentGroupInfo grpPropInfo;
            if (!tInfo.ArgumentGroups.TryGetValue(groupName, out grpPropInfo))
            {
                grpPropInfo = new ArgumentGroupInfo();
                tInfo.ArgumentGroups[groupName] = grpPropInfo;
            }

            return grpPropInfo;
        }

        private static PropertyInfo FindCommandProperty(PropertyInfo[] propertiesOnType)
        {
            PropertyInfo result = null;
            foreach (var prop in propertiesOnType)
            {
                if (prop.GetCustomAttribute<ActionArgumentAttribute>() != null)
                {
                    if (result != null)
                    {
                        throw new ArgumentException($"You can only define a single property as the command property");
                    }
                    result = prop;
                }
            }
            return result;
        }
    }
}
