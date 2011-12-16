
namespace RIATest.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // The MetadataTypeAttribute identifies CategoryMetadata as the class
    // that carries additional metadata for the Category class.
    [MetadataTypeAttribute(typeof(Category.CategoryMetadata))]
    public partial class Category
    {

        // This class allows you to attach custom attributes to properties
        // of the Category class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CategoryMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CategoryMetadata()
            {
            }

            public int CategoryId
            {
                get;
                set;
            }

            public string Description
            {
                get;
                set;
            }

            public EntityCollection<Task> Tasks
            {
                get;
                set;
            }
        }
    }

    // The MetadataTypeAttribute identifies SprintMetadata as the class
    // that carries additional metadata for the Sprint class.
    [MetadataTypeAttribute(typeof(Sprint.SprintMetadata))]
    public partial class Sprint
    {

        // This class allows you to attach custom attributes to properties
        // of the Sprint class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class SprintMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private SprintMetadata()
            {
            }

            public string Description
            {
                get;
                set;
            }

            public int SprintId
            {
                get;
                set;
            }

            [Include]
            public EntityCollection<Story> Stories
            {
                get;
                set;
            }
        }
    }

    // The MetadataTypeAttribute identifies StoryMetadata as the class
    // that carries additional metadata for the Story class.
    [MetadataTypeAttribute(typeof(Story.StoryMetadata))]
    public partial class Story
    {

        // This class allows you to attach custom attributes to properties
        // of the Story class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class StoryMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private StoryMetadata()
            {
            }

            public string Description
            {
                get;
                set;
            }

            public Sprint Sprint
            {
                get;
                set;
            }

            public int SprintId
            {
                get;
                set;
            }

            public int StoryId
            {
                get;
                set;
            }

            [Include]
            public EntityCollection<Task> Tasks
            {
                get;
                set;
            }
        }
    }

    // The MetadataTypeAttribute identifies TaskMetadata as the class
    // that carries additional metadata for the Task class.
    [MetadataTypeAttribute(typeof(Task.TaskMetadata))]
    public partial class Task
    {

        // This class allows you to attach custom attributes to properties
        // of the Task class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class TaskMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private TaskMetadata()
            {
            }

            public Category Category
            {
                get;
                set;
            }

            public int CategoryId
            {
                get;
                set;
            }

            public string Description
            {
                get;
                set;
            }

            public Story Story
            {
                get;
                set;
            }

            public int StoryId
            {
                get;
                set;
            }

            public int TaskId
            {
                get;
                set;
            }
        }
    }
}
