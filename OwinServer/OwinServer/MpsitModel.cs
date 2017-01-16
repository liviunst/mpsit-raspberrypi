namespace OwinServer
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MpsitModel : DbContext
    {
        // Your context has been configured to use a 'MpsitModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'OwinServer.MpsitModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MpsitModel' 
        // connection string in the application configuration file.
        public MpsitModel()
            : base("name=MpsitModel")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Command> Commands { get; set; }
        public virtual DbSet<Temperature> Temperatures { get; set; }
        public virtual DbSet<Image> Images { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

    public class Command
    {
        public int Id { get; set; }
        public bool Sent { get; set; }
        public string Value { get; set; }
    }

    public class Temperature
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }

    public class Image
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public string Name { get; set; }
    }
}