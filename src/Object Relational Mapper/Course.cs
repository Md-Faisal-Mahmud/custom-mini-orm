using Object_Relational_Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Object_Relational_Mapper
{
    #region Course class int
    //public class Course : IEntity<int>
    //{
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int Id { get; set; }
    //    public string Title { get; set; }
    //    public Instructor Teacher { get; set; }
    //    public List<Topic> Topics { get; set; }
    //    public double Fees { get; set; }
    //    public List<AdmissionTest> Tests { get; set; }

    //    public Course()
    //    {
    //        Id = 1;  
    //        Title = "Asp.net C#";
    //        Teacher = new Instructor()
    //        {
    //            Id = 1,  
    //            Name = "Jalal Uddin",
    //            Email = "jalaluddin@devskill.com",
    //            PresentAddress = new Address()
    //            {
    //                Id = 1,  
    //                Street = "Mirpur-2",
    //                City = "Dhaka",
    //                Country = "Bangladesh"
    //            },
    //            PermanentAddress = new Address()
    //            {
    //                Id = 2,  
    //                Street = "Moghbazar",
    //                City = "Dhaka",
    //                Country = "Bangladesh"
    //            },
    //            PhoneNumbers = new List<Phone>
    //            {
    //                new Phone(){ Id = 1, Number = "828320328", Extension = "382", CountryCode = "555" },
    //                new Phone(){ Id = 2, Number = "304303343", Extension = "454", CountryCode = "343" },
    //            }
    //        };
    //        Fees = 30000.5;
    //        Topics = new List<Topic>()
    //            {
    //                new Topic
    //                {
    //                    Id = 1,  
    //                    Title = "Gettig Started",
    //                    Description = "Frist Demo",
    //                    Sessions = new List<Session>
    //                    {
    //                        new Session{ Id = 1, DurationInHour = 2, LearningObjective = "Start learning" },
    //                        new Session{ Id = 2, DurationInHour = 3, LearningObjective = "Write Code" },
    //                        new Session{ Id = 3, DurationInHour = 4, LearningObjective = "Run Code" },
    //                    }
    //                },
    //                new Topic
    //                {
    //                    Id = 2,  
    //                    Title = "Installation",
    //                    Description = "Tools",
    //                    Sessions = new List<Session>
    //                    {
    //                        new Session{ Id = 4, DurationInHour = 1, LearningObjective = "VS Code" },
    //                        new Session{ Id = 5, DurationInHour = 4, LearningObjective = "Docker" },
    //                        new Session{ Id = 6, DurationInHour = 2, LearningObjective = "Git" },
    //                    }
    //                },
    //                new Topic
    //                {
    //                    Id = 3,  
    //                    Title = "Project",
    //                    Description = "Build Application",
    //                    Sessions = new List<Session>
    //                    {
    //                        new Session{ Id = 7, DurationInHour = 2, LearningObjective = "Start learning" },
    //                        new Session{ Id = 8, DurationInHour = 3, LearningObjective = "Write Code" },
    //                        new Session{ Id = 9, DurationInHour = 4, LearningObjective = "Run Code" },
    //                    }
    //                },
    //            };
    //        Tests = new List<AdmissionTest>()
    //        {
    //            new AdmissionTest()
    //            {
    //                Id = 1,  
    //                TestFees = 100.5,
    //                StartDateTime = new DateTime(2022, 2, 3),
    //                EndDateTime = new DateTime(2022, 2, 4)
    //            },
    //            new AdmissionTest
    //            {
    //                Id = 2,  
    //                TestFees = 200.5,
    //                StartDateTime = new DateTime(2023, 4, 3),
    //                EndDateTime = new DateTime(2023, 4, 4)
    //            },
    //            new AdmissionTest
    //            {
    //                Id = 3,  
    //                TestFees = 300.5,
    //                StartDateTime = new DateTime(2024, 5, 3),
    //                EndDateTime = new DateTime(2024, 5, 4)
    //            }
    //        };
    //    }
    //}

    //public class AdmissionTest
    //{
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int Id { get; set; }
    //    public DateTime StartDateTime { get; set; }
    //    public DateTime EndDateTime { get; set; }
    //    public double TestFees { get; set; }
    //}

    //public class Topic
    //{
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int Id { get; set; }
    //    public string Title { get; set; }
    //    public string Description { get; set; }
    //    public List<Session> Sessions { get; set; }

    //}

    //public class Session
    //{
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int Id { get; set; }
    //    public int DurationInHour { get; set; }
    //    public string LearningObjective { get; set; }
    //}

    //public class Instructor
    //{
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //    public Address PresentAddress { get; set; }
    //    public Address PermanentAddress { get; set; }
    //    public List<Phone> PhoneNumbers { get; set; }
    //}

    //public class Address
    //{
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int Id { get; set; }
    //    public string Street { get; set; }
    //    public string City { get; set; }
    //    public string Country { get; set; }
    //}

    //public class Phone
    //{
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int Id { get; set; }
    //    public string Number { get; set; }
    //    public string Extension { get; set; }
    //    public string CountryCode { get; set; }
    //}
    #endregion

    #region Course class Guid
    public class Course : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Instructor Teacher { get; set; }
        public List<Topic> Topics { get; set; }
        public double Fees { get; set; }
        public List<AdmissionTest> Tests { get; set; }

        public Course()
        {
            Id = new Guid("320035e0-4bfb-41fc-b79b-eee02dc97bdd");
            Title = " asp net C#";
            Teacher = new Instructor()
            {
                Id = new Guid("9f7c35ac-c0aa-40ef-a39f-87aa59d52f9e"),
                Name = "Md. Jalal Uddin",
                Email = "jalaluddin@devskill.com",
                PresentAddress = new Address()
                {
                    Id = new Guid("e2a2e9f6-a4ab-45e6-83c3-5f37f3f2a6f3"),
                    Street = "Mirpur-2",
                    City = "Dhaka",
                    Country = "Bangladesh"
                },
                PermanentAddress = new Address()
                {
                    Id = new Guid("e1ee437e-188c-4be8-9e05-e68a7e51a6c4"),
                    Street = "Moghbazar",
                    City = "Dhaka",
                    Country = "Bangladesh"
                },
                PhoneNumbers = new List<Phone>
                    {
                        new Phone(){ Id = new Guid("68d3d046-c6f8-4997-9311-0eb188cb8ae0"), Number = "828320328", Extension = "382", CountryCode = "555" },
                        new Phone(){ Id = new Guid("e0929947-8e3a-4d58-b27e-9684a0d8cd3c"), Number = "304303343", Extension = "454", CountryCode = "343" },
                    }
            };
            Fees = 30000.5;
            Topics = new List<Topic>()
                    {
                        new Topic
                        {
                            Id = new Guid("1595a0c2-264d-460f-904c-86309620ed90"),
                            Title = "Gettig Started",
                            Description = "Frist Demo",
                            Sessions = new List<Session>
                            {
                                new Session{ Id = new Guid("c74db82d-db06-400f-863d-a162b0423514"), DurationInHour = 2, LearningObjective = "Start learning1" },
                                new Session{ Id = new Guid("3ddd19ce-18ec-4c2e-ba16-5707ee312c44"), DurationInHour = 3, LearningObjective = "Write Code" },
                                new Session{ Id = new Guid("664fdb93-152c-4f20-89cd-dbedad29263b"), DurationInHour = 4, LearningObjective = "Run Code" },
                            }
                        },
                        new Topic
                        {
                            Id = new Guid("9a7f05b5-d6cd-45a8-a52f-227054c3b8f3"),
                            Title = "Installation",
                            Description = "Tools",
                            Sessions = new List<Session>
                            {
                                new Session{ Id = new Guid("ce5b6637-0f28-4d71-a451-01040e7153c4"), DurationInHour = 1, LearningObjective = "VS Code" },
                                new Session{ Id = new Guid("03ed30d2-2df3-4576-b011-e3a6cc0033af"), DurationInHour = 4, LearningObjective = "Docker" },
                                new Session{ Id = new Guid("f0290873-a80c-4c5f-86ca-0da147b4b750"), DurationInHour = 2, LearningObjective = "Git" },
                            }
                        },
                        new Topic
                        {
                            Id = new Guid("c7375056-a142-4fde-b60d-1584465d43d2"),
                            Title = "Project",
                            Description = "Build Application",
                            Sessions = new List<Session>
                            {
                                new Session{ Id = new Guid("e5ae4fa2-3245-4e99-b26f-46c231abff88"), DurationInHour = 2, LearningObjective = "Start learning2" },
                                new Session{ Id = new Guid("691ac54b-6c5c-4da1-8249-729d8cdb8619"), DurationInHour = 3, LearningObjective = "Write Code" },
                                new Session{ Id = new Guid("37a91fef-96bc-41ee-9654-2767858ca01b"), DurationInHour = 4, LearningObjective = "Run Code" },
                            }
                        },
                    };
            Tests = new List<AdmissionTest>()
                {
                    new AdmissionTest()
                    {
                        Id = new Guid("8043622f-4056-45b4-b87d-e8b20f7ddb18"),
                        TestFees = 100.5,
                        StartDateTime = new DateTime(2022, 2, 3),
                        EndDateTime = new DateTime(2022, 2, 4)
                    },
                    new AdmissionTest
                    {
                        Id = new Guid("c8b2e5ac-1b1b-46ef-9a6a-fbaa1c30477c"),
                        TestFees = 200.5,
                        StartDateTime = new DateTime(2023, 4, 3),
                        EndDateTime = new DateTime(2023, 4, 4)
                    },
                    new AdmissionTest
                    {
                        Id =  new Guid("a3c2a5eb-a8e8-4bec-b072-0432f4e2e9ae"),
                        TestFees = 300.5,
                        StartDateTime = new DateTime(2024, 5, 3),
                        EndDateTime = new DateTime(2024, 5, 4)
                    }
                };
        }
    }

    public class AdmissionTest
    {
        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double TestFees { get; set; }
    }

    public class Topic
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Session> Sessions { get; set; }

    }

    public class Session
    {
        public Guid Id { get; set; }
        public int DurationInHour { get; set; }
        public string LearningObjective { get; set; }
    }

    public class Instructor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Address PresentAddress { get; set; }
        public Address PermanentAddress { get; set; }
        public List<Phone> PhoneNumbers { get; set; }
    }

    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class Phone
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string Extension { get; set; }
        public string CountryCode { get; set; }
    }

    #endregion

}

#region Product class Guid
//    public class Product : IEntity<Guid>
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public string BarCode { get; set; }
//        public string Description { get; set; }
//        public List<Feedback> Feedbacks { get; set; }
//        public Spedification[] Spedificatios { get; set; }
//        public decimal Price { get; set; }
//        public List<Color> Colors { get; set; }

//        public Product()
//        {
//            Id = Guid.NewGuid();
//            Name = "Camera";
//            Description = "A cannon camera";
//            BarCode = "0475047503";

//            Feedbacks = new List<Feedback>()
//                {
//                    new Feedback()
//                    {
//                        Id = Guid.NewGuid(),
//                        FeedbackProivdername = "Jalaluddin", Rating = 4.5,
//                        FeedbackItems = new FeedbackItem[]
//                        {
//                            new FeedbackItem() { Id = Guid.NewGuid(), Rating =  3.2, Name = "Durability"},
//                            new FeedbackItem() { Id = Guid.NewGuid(), Rating =  3.5, Name = "User Friendliness"},
//                        }
//                    },
//                    new Feedback()
//                    {
//                        Id = Guid.NewGuid(),
//                        FeedbackProivdername = "Tareq", Rating = 2.5,
//                        FeedbackItems = new FeedbackItem[]
//                        {
//                            new FeedbackItem() { Id = Guid.NewGuid(), Rating = 2.2, Name = "Durability"},
//                            new FeedbackItem() { Id = Guid.NewGuid(), Rating =  2.5, Name = "User Friendliness"},
//                        }
//                    }
//                };

//            Spedificatios = new Spedification[]
//            {
//                    new Spedification()
//                    {
//                        Id = Guid.NewGuid(),
//                        Items = new List<SpedificationItem>
//                        {
//                            new SpedificationItem() { Id = Guid.NewGuid(), Name = "Model", Value = "Cannon"},
//                            new SpedificationItem() { Id = Guid.NewGuid(), Name = "Pixel", Value = "12MPX"}
//                        }
//                    }
//            };

//            Price = 30000.5m;

//            Colors = new List<Color>()
//                {
//                    new Color() { Id = Guid.NewGuid(), Name = "FrontColor", Code = "Black"},
//                    new Color() { Id = Guid.NewGuid(), Name = "BackColor", Code = "White"}
//                };
//        }
//    }

//    public class SpedificationItem
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public string Value { get; set; }
//    }

//    public class Spedification
//    {
//        public Guid Id { get; set; }
//        public List<SpedificationItem> Items { get; set; }
//    }

//    public class Feedback
//    {
//        public Guid Id { get; set; }
//        public string FeedbackProivdername { get; set; }
//        public double Rating { get; set; }
//        public FeedbackItem[] FeedbackItems { get; set; }
//    }

//    public class FeedbackItem
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public double Rating { get; set; }
//    }

//    public class Color
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public string Code { get; set; }
//    }
//}


#endregion

#region Product class int
//    public class Product : IEntity<int>
//    {
//        [DatabaseGenerated(DatabaseGeneratedOption.None)]
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public string BarCode { get; set; }
//        public string Description { get; set; }
//        public List<Feedback> Feedbacks { get; set; }
//        public Spedification[] Spedificatios { get; set; }
//        public decimal Price { get; set; }
//        public List<Color> Colors { get; set; }

//        public Product()
//        {
//            Id = 1;
//            Name = "Camera";
//            Description = "A cannon camera";
//            BarCode = "0475047503";

//            Feedbacks = new List<Feedback>()
//                {
//                    new Feedback()
//                    {
//                        Id = 1,
//                        FeedbackProivdername = "Jalaluddin", Rating = 4.5,
//                        FeedbackItems = new FeedbackItem[]
//                        {
//                            new FeedbackItem() { Id = 1, Rating =  3.2, Name = "Durability"},
//                            new FeedbackItem() { Id = 2, Rating =  3.5, Name = "User Friendliness"},
//                        }
//                    },
//                    new Feedback()
//                    {
//                        Id = 2,
//                        FeedbackProivdername = "Tareq", Rating = 2.5,
//                        FeedbackItems = new FeedbackItem[]
//                        {
//                            new FeedbackItem() { Id = 3, Rating = 2.2, Name = "Durability"},
//                            new FeedbackItem() { Id = 4, Rating =  2.5, Name = "User Friendliness"},
//                        }
//                    }
//                };

//            Spedificatios = new Spedification[]
//            {
//                    new Spedification()
//                    {
//                        Id = 1,
//                        Items = new List<SpedificationItem>
//                        {
//                            new SpedificationItem() { Id = 1, Name = "Model", Value = "Cannon"},
//                            new SpedificationItem() { Id = 2, Name = "Pixel", Value = "50MPX"}
//                        }
//                    }
//            };

//            Price = 30000.5m;

//            Colors = new List<Color>()
//                {
//                    new Color() { Id = 1, Name = "FrontColor", Code = "Rgb"},
//                    new Color() { Id = 2, Name = "BackColor", Code = "White"}
//                };
//        }
//    }

//    public class SpedificationItem
//    {
//        [DatabaseGenerated(DatabaseGeneratedOption.None)]
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public string Value { get; set; }
//    }

//    public class Spedification
//    {
//        [DatabaseGenerated(DatabaseGeneratedOption.None)]
//        public int Id { get; set; }
//        public List<SpedificationItem> Items { get; set; }
//    }

//    public class Feedback
//    {
//        [DatabaseGenerated(DatabaseGeneratedOption.None)]
//        public int Id { get; set; }
//        public string FeedbackProivdername { get; set; }
//        public double Rating { get; set; }
//        public FeedbackItem[] FeedbackItems { get; set; }
//    }

//    public class FeedbackItem
//    {
//        [DatabaseGenerated(DatabaseGeneratedOption.None)]
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public double Rating { get; set; }
//    }

//    public class Color
//    {
//        [DatabaseGenerated(DatabaseGeneratedOption.None)]
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public string Code { get; set; }
//    }
//}


#endregion

#region my custom dataset Guid
//public class Course : IEntity<Guid>
//    {
//        public Guid Id { get; set; }
//        public string Title { get; set; }
//        public Instructor Teacher { get; set; }
//        public Spedification[] Spedificatios { get; set; }
//        public double Fees { get; set; }
//        public AdmissionTest[] Tests { get; set; }
//        public Course()
//        {
//            Id = Guid.NewGuid();
//            Title = "faisal";
//            Fees = 343.34;

//            Tests = new AdmissionTest[]
//                        {
//                            new AdmissionTest
//                            {
//                                Id = Guid.NewGuid(),
//                                TestFees = 100.5,
//                                StartDateTime = new DateTime(2022, 2, 3),
//                                EndDateTime = new DateTime(2022, 2, 4)
//                            },
//                            new AdmissionTest
//                            {
//                                Id = Guid.NewGuid(),
//                                TestFees = 200.5,
//                                StartDateTime = new DateTime(2023, 4, 3),
//                                EndDateTime = new DateTime(2023, 4, 4)
//                            },
//                            new AdmissionTest
//                            {
//                                Id = Guid.NewGuid(),
//                                TestFees = 300.5,
//                                StartDateTime = new DateTime(2024, 5, 3),
//                                EndDateTime = new DateTime(2024, 5, 4)
//                            }
//                        };
//            Spedificatios = new Spedification[]
//                {

//                        new Spedification()
//                        {
//                            Id = Guid.NewGuid(),
//                            Items = new List<SpedificationItem>
//                            {
//                                new SpedificationItem() { Id = Guid.NewGuid(),Name = "Model", Value = "Cannon"},
//                                new SpedificationItem() { Id = Guid.NewGuid(), Name = "Pixel", Value = "12MPX"}
//                            }
//                        }
//                };
//            Teacher = new Instructor()
//            {
//                Id = Guid.NewGuid(),
//                Name = "Md. Jalal Uddin",
//                Email = "jalaluddin@devskill.com",
//                PermanentAddress = new Address()
//                {
//                    Id = Guid.NewGuid(),
//                    Street = "Moghbazar",
//                    City = "Dhaka",
//                    Country = "Bangladesh"
//                },
//                PresentAddress = new Address()
//                {
//                    Id = Guid.NewGuid(),
//                    Street = "Mirpur-2",
//                    City = "Dhaka",
//                    Country = "Bangladesh"
//                },
//                PhoneNumbers = new List<Phone>
//                    {
//                                    new Phone(){ Id = Guid.NewGuid(), Number = "828320328", Extension = "382", CountryCode = "555" },
//                                    new Phone(){ Id = Guid.NewGuid(), Number = "304303343", Extension = "454", CountryCode = "343" },
//                    }
//            };


//        }






//    }
//    public class Spedification
//    {
//        public Guid Id { get; set; }
//        public List<SpedificationItem> Items { get; set; }
//    }
//    public class SpedificationItem
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public string Value { get; set; }
//    }
//    public class AdmissionTest
//    {
//        public Guid Id { get; set; }
//        public DateTime StartDateTime { get; set; }
//        public DateTime EndDateTime { get; set; }
//        public double TestFees { get; set; }
//    }
//    public class Instructor
//    {
//        public Guid Id { get; set; }
//        public string Name { get; set; }
//        public string Email { get; set; }
//        public Address PresentAddress { get; set; }
//        public Address PermanentAddress { get; set; }
//        public List<Phone> PhoneNumbers { get; set; }
//    }

//    public class Address
//    {
//        public Guid Id { get; set; }
//        public string Street { get; set; }
//        public string City { get; set; }
//        public string Country { get; set; }
//    }

//    public class Phone
//    {
//        public Guid Id { get; set; }
//        public string Number { get; set; }
//        public string Extension { get; set; }
//        public string CountryCode { get; set; }
//    }
//}

#endregion