using System;
using System.Collections.Generic;
using System.Linq;

namespace leat{
    public interface IMachine{
        void Start();
        void Stop();
    }

    public interface IMaintainable{
        void PerformMaintenance();
    }

    public interface IRechargeable{
        void RechargeBattery();
        int CheckBatteryLevel();
    }

    public interface IIdentifiable{
        Guid ID { get; }
    }

    public abstract class Machine : IMachine, IIdentifiable{
        public Guid ID { get; } = Guid.NewGuid();
        public string Manufacturer { get; protected set; }
        public string Model { get; protected set; }
        public int Year { get; protected set; }

        public abstract void Start();
        public abstract void Stop();

        public void GetInfo(){
            Console.WriteLine($"ID:{ID} Manufacturer:{Manufacturer} Model:{Model} Year:{Year}");
        }
    }

    public class Robot : Machine{
        public Robot(string manufacturer, string model, int year){
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
        }

        public override void Start() => Console.WriteLine($"Robot {ID} starting");
        public override void Stop() => Console.WriteLine($"Robot {ID} stopping");
    }

    public class Vehicle : Machine, IMaintainable{
        public Vehicle(string manufacturer, string model, int year){
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
        }

        public override void Start() => Console.WriteLine($"Vehicle {ID} starting");
        public override void Stop() => Console.WriteLine($"Vehicle {ID} stopping");
        public void PerformMaintenance() => Console.WriteLine($"Vehicle {ID} maintenance");
    }

    public class Drone : Machine, IMaintainable, IRechargeable{
        public int batterylevel { get; private set; }

        public Drone(string manufacturer, string model, int year){
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            batterylevel = 0;
        }

        public override void Start() => Console.WriteLine($"Drone {ID} starting");
        public override void Stop() => Console.WriteLine($"Drone {ID} stopping");
        public void PerformMaintenance() => Console.WriteLine($"Drone {ID} maintenance");
        public void RechargeBattery() => Console.WriteLine($"Drone {ID} charging -> {batterylevel += 10}%");
        public int CheckBatteryLevel() => batterylevel;
    }

    public class Conveyor : IMachine, IMaintainable, IIdentifiable{
        public Guid ID { get; } = Guid.NewGuid();
        public void Start() => Console.WriteLine($"Conveyor {ID} starting");
        public void Stop() => Console.WriteLine($"Conveyor {ID} stopping");
        void IMaintainable.PerformMaintenance() => Console.WriteLine($"Conveyor {ID} maintenance");
    }

    public class Program{
        public static void Main(){
            List<IMachine> machines = new List<IMachine>{
                new Robot("robocorp","x1",2020),
                new Vehicle("autoinc","v200",2018),
                new Drone("flyhigh","d4",2021),
                new Conveyor()
            };
            foreach(IMachine m in machines) m.Start();
            foreach(IMachine m in machines) m.Stop();
            List<IMaintainable> maintainables = machines.OfType<IMaintainable>().ToList();
            foreach(IMaintainable m in maintainables) m.PerformMaintenance();
            List<IRechargeable> rechargeables = machines.OfType<IRechargeable>().ToList();
            foreach(IRechargeable r in rechargeables){
                r.RechargeBattery();
                Console.WriteLine(r.CheckBatteryLevel());
            }
            List<IIdentifiable> identifiables = machines.OfType<IIdentifiable>().ToList();
            foreach(IIdentifiable i in identifiables) Console.WriteLine($"found id {i.ID}");
            Console.ReadLine();
        }
    }
}
