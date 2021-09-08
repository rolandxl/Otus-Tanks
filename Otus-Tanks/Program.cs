using System;
using System.Numerics;
using System.Collections.Generic;

namespace Otus_Tanks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Старт программы");

			Tank tank = new();

			ICommand move = new Move(new MovableAdapter(tank));
			Console.WriteLine($"Инициализация механики движения танка прошла {move}");

			tank.ClearPropertys();
			tank.SetProperty("position", new Vector3(1, 0, 1));
			Console.WriteLine($"Установим только position и подвинем");
			Console.WriteLine($"position: {tank.GetProperty("position")}, velocity: {tank.GetProperty("velocity")}");

			try
            {
				move.Execute();
			}
            catch (Exception e)
            {
				Console.WriteLine(e.Message);
			}

			tank.ClearPropertys();
			tank.SetProperty("velocity", new Vector3(0, 1, 0));
			Console.WriteLine($"Установим только velocity и подвинем");
			Console.WriteLine($"position: {tank.GetProperty("position")}, velocity: {tank.GetProperty("velocity")}");

			try
			{
				move.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}


			tank.SetProperty("position", new Vector3(12,0,5));
			tank.SetProperty("velocity", new Vector3(-7,0,3));
			Console.WriteLine($"Начальная инициализация свойств танка прошла, position: {new Vector3(12, 0, 5)}, velocity: {new Vector3(-7, 0, 3)}");

			move.Execute();
			Console.WriteLine($"Выполнили механику движение. position: {tank.GetProperty("position")}, velocity: {tank.GetProperty("velocity")}");


			ICommand rotate = new Rotate(new RotateableAdapter(tank));
			Console.WriteLine($"Инициализация механики поворота танка прошла {rotate}");

			
			tank.ClearPropertys();
			tank.SetProperty("angle", new Vector3(0, 3, 0));
			Console.WriteLine($"Установим только angle и повернем");
			Console.WriteLine($"angle: {tank.GetProperty("angle")}, rotation: {tank.GetProperty("rotation")}");

			try
			{
				rotate.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}


			tank.ClearPropertys();
			tank.SetProperty("rotation", new Vector3(0, 10, 0));
			Console.WriteLine($"Установим только rotation и повернем");
			Console.WriteLine($"angle: {tank.GetProperty("angle")}, rotation: {tank.GetProperty("rotation")}");

			try
			{
				rotate.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}



			Console.ReadLine();

		}
	}

	//создаем интерфейс универсального объекта контейнера, от которого будет наследоваться всё что нам необходимо
	interface IUObject
    {
		object GetProperty(string key);
		void SetProperty(string key, object value);		
	}


	//реализуем интерфейс в класс 
	class Tank : IUObject
	{
		public Tank()
		{
			Propertys = new();
		}

		public object GetProperty(string key)
		{
			Propertys.TryGetValue(key, out object value);
            return value;
        }
		public void SetProperty(string key, object value)
        {
			if (Propertys.ContainsKey(key))
				Propertys[key] = value;
			else Propertys.TryAdd(key, value);	
        }

		public void ClearPropertys()
        {
			Propertys.Clear();
		}
		Dictionary<string, object> Propertys;
	}


	//создаем интерфейс команды для последующей реалиции его через конкретные механики 
	interface ICommand
	{
		void Execute();
	}

	//создаем интерфейс объекта который движется
	interface IMovable
	{
		Vector3 GetPosition();
		void SetPosition(Vector3 newValue);
		Vector3 GetVelocity();
	}


	//создаем класс-адаптер реализующий интерфейс движение 
	class MovableAdapter : IMovable
	{
        readonly IUObject Obj;
		public MovableAdapter(IUObject Obj)
		{
			this.Obj = Obj;
		}

		public Vector3 GetPosition()
		{
			if (Obj.GetProperty("position") == null) throw new Exception("Error: position is null");
			return (Vector3)Obj.GetProperty("position");
		}
		public void SetPosition(Vector3 newValue)
		{
			Obj.SetProperty("position", newValue);
		}
		public Vector3 GetVelocity()
		{
			if (Obj.GetProperty("velocity") == null) throw new Exception("Error: velocity is null");
			return (Vector3)Obj.GetProperty("velocity");
		}
	}

	//создаем реализацию команды перемещения в виде класса
	class Move : ICommand
	{
        readonly IMovable movable; 
		public Move(IMovable movable)
		{
			this.movable = movable;
		}

		public void Execute()
		{
			movable.SetPosition(movable.GetPosition() + movable.GetVelocity());
		}
	}

	interface IRotateable
	{
		Vector3 GetAngle();
		void SetAngle(Vector3 newValue);
		Vector3 GetRotation();
	}

	class RotateableAdapter : IRotateable
	{
		readonly IUObject Obj;
		public RotateableAdapter(IUObject Obj)
		{
			this.Obj = Obj;
		}

		public Vector3 GetAngle()
		{
			if (Obj.GetProperty("angle") == null) throw new Exception("Error: angle is null");
			return (Vector3)Obj.GetProperty("angle");
		}
		public void SetAngle(Vector3 newValue)
		{
			Obj.SetProperty("angle", newValue);
		}
		public Vector3 GetRotation()
		{
			if (Obj.GetProperty("rotation") == null) throw new Exception("Error: rotation is null");
			return (Vector3)Obj.GetProperty("rotation");
		}
	}


	class Rotate : ICommand
	{
		readonly IRotateable rotateable;
		public Rotate(IRotateable rotateable)
		{
			this.rotateable = rotateable;
		}

		public void Execute()
		{
			rotateable.SetAngle(rotateable.GetAngle() + rotateable.GetRotation());
		}
	}






	/*
	interface IMovable
		{
			Vector getPosition();
			void setPosition(Vector newValue);
			Vector getVelocity();

		}

		interface IRotable
		{

		}

		interface UObject
		{
			object getProperty(string key);
			void setProperty(string key, object value);
		}

		class MovableAdapter : IMovable
		{
			UObject obj;
			public MovableAdapter(UObject obj)
			{
				this.obj = obj;
			}

			public Vector getPosition()
			{
				return (Vector)obj.getProperty("position");
			}
			public void setPosition(Vector newValue)
			{
				obj.setProperty("position", newValue);
			}
			public Vector getVelocity()
			{
				return (Vector)obj.getProperty("velocity");
			}
		}

	"public $(T) get$(name)() { return $(T)obj.getProperty(\"$(name)\");}"


	Command move = new Move(new MovableAdapter(obj));

		Command move = new MacroCommand({
		new CheckFuel(new CheckFueableAdapter(obj)),
			new Move(new MovableAdapter(obj)),
			new BurnFuel(new CBurnFueableAdapter(obj))
	});


	Command move = IoC.resolve<Command>("move", obj);

	move.execute();

	interface Command
	{
		void execute();
	}

	class CheckFuelAdapter : ICheckFueable
	{
		UObject obj;
		public CheckFueableAdapter(UObject obj)
		{
			this.obj = obj;
		}

		public unsigned int getFuel()
		{
			return IoC.resolve<int>("CheckFuel_fuel", obj);
		}
		public int getFuelVelocity()
		{
			return (int)obj.getProperty("fuelVelocity");
		}
	}


	class CheckFuel : Command
	{
		ICheckFuelable checkFueable;

		public CheckFuel(ICheckFuelable checkfueable)
		{
			this.checkFueable = checkfueable;
		}

		public void execute()
		{
			if (checkfueable.getFuel() - checkfueable.fuelVelocity() < 0)
				throw new CommandException();
		}
	}

	class BurnFuel : Command
	{
		IBurnFueable burnFueable;

		public BurnFuel(IBurnFuelable burnfueable)
		{
			this.checkFueable = burnfueable;
		}

		public void execute()
		{
			burnfueable.setFuel(burnFueable.getFuel() - checkfueable.fuelVelocity());
		}
	}


	class Move : Command
	{
		IMovable movable;
		public Move(IMovable movable)
		{
			this.movable = movable;
		}

		public void Execute()
		{
			movable.setPosition(movable.getPostion() + movable.getVelocity());
		}
	}*/


}
