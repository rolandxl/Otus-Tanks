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
			tank.SetProperty("angle", 30f);
			tank.SetProperty("axis", new Vector3(0, 1, 0));
			Console.WriteLine($"Установим только angle и axis и повернем");
			Console.WriteLine($"rotation: {tank.GetProperty("rotation")}, axis: {tank.GetProperty("axis")}, angle: {tank.GetProperty("angle")}");

			try
			{
				rotate.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}


			tank.ClearPropertys();
			tank.SetProperty("rotation", new Quaternion());
			tank.SetProperty("axis", new Vector3(0, 1, 0));
			Console.WriteLine($"Установим только rotation и axis и повернем");
			Console.WriteLine($"rotation: {tank.GetProperty("rotation")}, axis: {tank.GetProperty("axis")}, angle: {tank.GetProperty("angle")}");

			try
			{
				rotate.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}


			tank.ClearPropertys();
			tank.SetProperty("rotation", new Quaternion());
			tank.SetProperty("angle", 30f);
			Console.WriteLine($"Установим только rotation и angle и повернем");
			Console.WriteLine($"rotation: {tank.GetProperty("rotation")}, axis: {tank.GetProperty("axis")}, angle: {tank.GetProperty("angle")}");

			try
			{
				rotate.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			tank.ClearPropertys();
			tank.SetProperty("rotation", new Quaternion(1,1,1,1));
			tank.SetProperty("angle", 30f);
			tank.SetProperty("axis", new Vector3(0, 1, 0));
			Console.WriteLine($"Установим все значения поворота и повернем");
			Console.WriteLine($"rotation: {tank.GetProperty("rotation")}, axis: {tank.GetProperty("axis")}, angle: {tank.GetProperty("angle")}");

			try
			{
				rotate.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.WriteLine($"rotation: {tank.GetProperty("rotation")}, axis: {tank.GetProperty("axis")}, angle: {tank.GetProperty("angle")}");



			tank.SetProperty("angle", -90f);
			tank.SetProperty("axis", new Vector3(0, 1, 0));
			Console.WriteLine($"Повернем отсительно текущего положения");
			Console.WriteLine($"rotation: {tank.GetProperty("rotation")}, axis: {tank.GetProperty("axis")}, angle: {tank.GetProperty("angle")}");

			try
			{
				rotate.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.WriteLine($"rotation: {tank.GetProperty("rotation")}, axis: {tank.GetProperty("axis")}, angle: {tank.GetProperty("angle")}");


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

	//всё тоже самое с вращением

	interface IRotateable
	{
		Quaternion GetRotation();
		void SetRotation(Quaternion newValue);
		void GetRotate(ref Vector3 axis, ref float angle);
	}

	class RotateableAdapter : IRotateable
	{
		readonly IUObject Obj;
		public RotateableAdapter(IUObject Obj)
		{
			this.Obj = Obj;
		}

		public Quaternion GetRotation()
		{
			if (Obj.GetProperty("rotation") == null) throw new Exception("Error: rotation is null");
			return (Quaternion)Obj.GetProperty("rotation");
		}
		public void SetRotation(Quaternion newValue)
		{
			Obj.SetProperty("rotation", newValue);
		}
		public void GetRotate(ref Vector3 axis, ref float angle)
		{
			if (Obj.GetProperty("axis") == null) throw new Exception("Error: axis is null");
			else if (Obj.GetProperty("angle") == null) throw new Exception("Error: angle is null");

			axis = (Vector3)Obj.GetProperty("axis");
			angle = (float)Obj.GetProperty("angle");
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
			Vector3 axis = new();
			float angle = 0;
			rotateable.GetRotate(ref axis, ref angle);
			rotateable.SetRotation(rotateable.GetRotation() * Quaternion.CreateFromAxisAngle(axis, angle));			
		}
	}
}
