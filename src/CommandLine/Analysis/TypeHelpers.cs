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
    internal class TypeHelpers
    {
        public static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().BaseType == typeof(Enum);
        }
        public static void ScanTypeForProperties<TOptions>(out TypeArgumentInfo tInfo)
        {
            tInfo = new TypeArgumentInfo();
            PropertyInfo[] propertiesOnType = typeof(TOptions).GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            // first of all, find the commandArgument, if any.
            tInfo.ActionArgument = FindCommandProperty(propertiesOnType);

            // parse the rest of the properties
            foreach (var property in propertiesOnType)
            {
                // get the group containing this property (note: more than one group can have the same property)
                // this allows common required parameters

                var groupsWhereThePropertyIsParticipating = GetGroupsForProperty(tInfo, property);

                var actualAttribs = property.GetCustomAttributes<ActualArgumentAttribute>().ToList();
                if (actualAttribs.Count > 1)
                {
                    throw new ArgumentException($"Only one of Required/Optional attribute are allowed per property ({property.Name}). [Red!Help information might be incorrect!]");
                }

                // if we have no attributes on that property, move on
                ActualArgumentAttribute baseAttrib = actualAttribs.FirstOrDefault();
                if (baseAttrib == null)
                {
                    continue;
                }

                // add the property to add the groups it is a part of
                if (baseAttrib is RequiredArgumentAttribute)
                {
                    foreach (ArgumentGroupInfo grpPropInfo in groupsWhereThePropertyIsParticipating)
                    {
                        if (grpPropInfo.RequiredArguments.ContainsKey((int)baseAttrib.GetArgumentId()))
                        {
                            throw new ArgumentException("Two required arguments share the same position!!");
                        }

                        grpPropInfo.RequiredArguments[(int)baseAttrib.GetArgumentId()] = property;
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

            ArgumentGroupInfo grp;
            // remove the empty one, if empty
            if (tInfo.ArgumentGroups.TryGetValue(string.Empty, out grp))
            {
                if (grp.OptionalArguments.Count == 0 && grp.RequiredArguments.Count == 0)
                    tInfo.ArgumentGroups.Remove(string.Empty);
            }

            // we want to be able to support empty groups.
            if (tInfo.ActionArgument != null)
            {
                // if it is an enum, and only then!
                if (IsEnum(tInfo.ActionArgument.PropertyType))
                {
                    // get the values of the enum.
                    var enumValues = Enum.GetValues(tInfo.ActionArgument.PropertyType);

                    foreach (var val in enumValues)
                    {
                        if (!tInfo.ArgumentGroups.ContainsKey(val.ToString()))
                        {
                            tInfo.ArgumentGroups.Add(val.ToString(), new ArgumentGroupInfo());
                        }
                    }
                }
            }
        }

        private static List<ArgumentGroupInfo> GetGroupsForProperty(TypeArgumentInfo tInfo, PropertyInfo property)
        {
            List<ArgumentGroupInfo> groupsForThisProperty = new List<ArgumentGroupInfo>();

            var customAttributes = property.GetCustomAttributes<ArgumentGroupAttribute>();

            if (!customAttributes.Any())
            {
                // we have the simple case where we don't have groups defined
                groupsForThisProperty.Add(GetArgumentInfoForSingleGroup(string.Empty, tInfo));
            }
            else
            {
                // we have the complex scenario where groups are present
                groupsForThisProperty.AddRange(GetArgumentInfoForGroups(tInfo, customAttributes));
            }

            return groupsForThisProperty;
        }

        private static IEnumerable<ArgumentGroupInfo> GetArgumentInfoForGroups(TypeArgumentInfo tInfo, IEnumerable<ArgumentGroupAttribute> customAttributes)
        {
            foreach (var commandGroup in customAttributes)
            {
                ArgumentGroupInfo grpPropInfo;
                if (!tInfo.ArgumentGroups.TryGetValue(commandGroup.Name, out grpPropInfo))
                {
                    grpPropInfo = new ArgumentGroupInfo();
                    tInfo.ArgumentGroups[commandGroup.Name] = grpPropInfo;
                }
                yield return grpPropInfo;
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
