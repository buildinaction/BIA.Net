using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using Microsoft.AspNet.Scaffolding.EntityFramework;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIA.CRUDScaffolder.MetaData
{
    public static class MetaDataBuilder
    {

        public static ModelMetadata PrepareMetaDataWithoutEntity(CodeType code, IEnumerable<CodeType> AllCodeTypes, out Dictionary<string, string>  dictFormat)
        {
            dictFormat = new Dictionary<string, string>();
            ModelMetadata modelMetadata = new ModelMetadata();
            List<CodeProperty> codeProperties = code.Members.OfType<CodeProperty>().ToList();
            List<PropertyMetadata> modelProperties = codeProperties.Select(p => ToPropertyMetaData(code.Name, p)).ToList();

            foreach (CodeProperty cp in codeProperties)
            {
                //set IsAssociation, AssociationDirection and RelatedModel
                PropertyMetadata mp = modelProperties.Where(m => m.PropertyName == cp.Name).SingleOrDefault();

                if (!cp.Type.IsPrimitiveType())
                {
                    if ((!(cp.Type.AsFullName.Length > 7 && cp.Type.AsFullName.Substring(0, 7) == "System.")) || cp.Type.CodeType.Name == "ICollection")
                        if ((cp.Type.AsString != "System.Collections.Generic.ICollection<int>") && (cp.Type.AsString != "System.Collections.Generic.ICollection<string>"))
                        {
                            mp.IsAssociation = true;
                            mp.AssociationDirection = AssociationDirection.ManyToOne;
                            if (cp.Type.CodeType.Name == "ICollection") mp.AssociationDirection = AssociationDirection.OneToMany;

                            mp.RelatedModel = new RelatedModelMetadata();
                            mp.RelatedModel.ForeignKeyPropertyNames = new string[1] { mp.PropertyName };

                            PropertyMetadata ForeignKeyProp = null;
                            if (cp.Type.CodeType.Name == "ICollection") ForeignKeyProp  =ExistProperty(modelProperties, mp.PropertyName + "Ids");
                            else ForeignKeyProp = ExistProperty(modelProperties, mp.PropertyName + "Id");

                            if (ForeignKeyProp != null)
                            {
                                ForeignKeyProp.IsForeignKey = true;
                                mp.RelatedModel.ForeignKeyPropertyNames = new string[1] { ForeignKeyProp.PropertyName };
                            }

                            string relativePropTypeName = cp.Type.AsString;
                            int index = relativePropTypeName.IndexOf(".ICollection<");
                            if (index > -1)
                            {
                                relativePropTypeName = relativePropTypeName.Substring(index + 13, relativePropTypeName.Length - index - 14);
                                mp.RelatedModel.EntitySetName = relativePropTypeName.Split('.').Last();
                            }
                            else
                            {
                                mp.RelatedModel.EntitySetName = cp.Type.CodeType.Name;
                            }
                            mp.RelatedModel.AssociationPropertyName = cp.Name;

                            CodeType codeRelative = AllCodeTypes
                           .Where(codeType => codeType.IsValidWebProjectEntityType() && codeType.FullName == relativePropTypeName).SingleOrDefault();
                            if (codeRelative != null)
                            {
                                List<CodeProperty> relativeCodeProperties = codeRelative.Members.OfType<CodeProperty>().ToList();
                                List<PropertyMetadata> relativeModelProperties = relativeCodeProperties.Select(p => ToPropertyMetaData(codeRelative.Name, p)).ToList();

                                mp.RelatedModel.PrimaryKeyNames = relativeModelProperties.Where(p => p.IsPrimaryKey).Select(p => p.PropertyName).ToArray();
                                PropertyMetadata DisplayProp = relativeModelProperties.Where(p => p.ShortTypeName == "string").FirstOrDefault();
                                mp.RelatedModel.DisplayPropertyName = DisplayProp == null ? "ToBeDefine" : DisplayProp.PropertyName;
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("The relative property '" + relativePropTypeName + "' is not defined. Please define all class before use CRUD.");
                                throw new Exception("The relative property '" + relativePropTypeName + "' is not defined.");
                            }

                        }
                    // IsAssociation
                }


                //Set IsForeignKey
                if (cp.Attributes.OfType<CodeAttribute>().Any(e => e.Name == "ForeignKey"))
                {
                    //CodeElement elem = cp.Attributes.OfType<CodeElement>().Where(e => e.Name == "ForeignKey").FirstOrDefault();
                    CodeAttribute elem = cp.Attributes.OfType<CodeAttribute>().Where(e => e.Name == "ForeignKey").FirstOrDefault();
                    string fullName = elem.Value.Substring(1, elem.Value.Length - 2);
                    PropertyMetadata ForeignKeyProp = ExistProperty(modelProperties, fullName);
                    if (ForeignKeyProp != null)
                    {
                        ForeignKeyProp.IsForeignKey = true;
                        mp.RelatedModel.ForeignKeyPropertyNames = new string[1] { ForeignKeyProp.PropertyName };
                    }
                }
                /*
                if (cp.Attributes.OfType<DisplayFormatAttribute>().Any())
                {
                    string format = cp.Attributes.OfType<DisplayFormatAttribute>().Select(a => a.DataFormatString).FirstOrDefault();
                    int i = 0;
                }*/

                if (cp.Attributes.OfType<CodeAttribute>().Any(e => e.Name == "DisplayFormat"))
                {
                    CodeAttribute elem = cp.Attributes.OfType<CodeAttribute>().Where(e => e.Name == "DisplayFormat").FirstOrDefault();
                    string argment = elem.Value;
                    string[] argments = argment.Split(',');
                    foreach(string arg in argments)
                    {
                        string[] keyVal = arg.Split('=');
                        if (keyVal[0].Trim() == "DataFormatString")
                        {
                            dictFormat.Add(mp.PropertyName, keyVal[1].Trim().Replace("\"",""));
                        }
                    }
                }
            }

            modelMetadata.EntitySetName = code.Name ;// _viewModel.SelectedModelType.ShortTypeName;
            modelMetadata.Properties = modelProperties.ToArray();
            modelMetadata.RelatedEntities = modelProperties.Where(p => p.AssociationDirection==AssociationDirection.ManyToOne && p.RelatedModel != null).Select(p => p.RelatedModel).ToArray();
            modelMetadata.PrimaryKeys = modelProperties.Where(p => p.IsPrimaryKey).ToArray();

            //var relatedMultiProperties = modelMetadata.Properties.Where(p => p.AssociationDirection == AssociationDirection.OneToMany && p.RelatedModel != null).Select(p => p.RelatedModel).ToDictionary(item => item.AssociationPropertyName);

            if (modelMetadata.PrimaryKeys == null || modelMetadata.PrimaryKeys.Length == 0)
            {
                System.Windows.MessageBox.Show("The primary key is not defined. Please precise the primary key in DTO by using [Key] data annotation.");
                throw new Exception("The primary key is not defined.");

            }

            return modelMetadata;
        }

        private static PropertyMetadata ToPropertyMetaData(string className, CodeProperty p)
        {
            PropertyMetadata prop = new PropertyMetadata();

            prop.PropertyName = p.Name;

            if (p.Name.ToLower() == "id" || p.Name.ToLower() == className.ToLower() + "id" || p.Attributes.OfType<CodeElement>().Any(e => e.Name == "Key"))
            {
                prop.IsPrimaryKey = true;
                prop.IsAutoGenerated = true;
            }
            else prop.IsPrimaryKey = false;
            prop.TypeName = p.Type.AsFullName;
            prop.ShortTypeName = p.Type.AsString;
            prop.Scaffold = true;

            return prop;
        }
        private static PropertyMetadata ExistProperty(List<PropertyMetadata> modelProperties, string propNameToFind)
        {
            foreach (PropertyMetadata m in modelProperties)
                if (m.PropertyName == propNameToFind) return m;
            return null;
        }

        /// <summary>
        /// x.[propertyName]
        /// </summary>
        /// <typeparam name="DTO"></typeparam>
        /// <param name="propertyName"></param>
        public static string DisplayFormat(Type DTOType, string propertyName)
        {
            propertyName = propertyName.Replace("__", ".");
            PropertyInfo propertyInfo = null;
            foreach (var property in propertyName.Split('.'))
            {
                propertyInfo = DTOType.GetProperty(property);
                DTOType = propertyInfo.PropertyType;
            }

            if (propertyInfo != null)
            {
                return (string)propertyInfo.CustomAttributes.Where(c => c.AttributeType.Name == "DisplayFormatAttribute").Select(c => c.NamedArguments.Where(n => n.MemberName == "DataFormatString").Select(n => n.TypedValue.Value).FirstOrDefault()).FirstOrDefault();
            }

            return null;
        }
    }
}
