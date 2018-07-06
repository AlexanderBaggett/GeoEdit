using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEdit
{
    public class Templates
    {
        public class GeoScene
        {
            List<GeoPoint> Points { get; set; }
            List<Polygon> Polygons { get; set; }
            List<Group> Groups { get; set; }

            public T GetByName<T>(string Name) where T : class, new()
            {
                switch (new T())
                {
                    case GeoPoint p: return this.Points.Where(x => x.Name == Name).First() as T;
                    case Polygon p: return this.Polygons.Where(x => x.Name == Name).First() as T;
                    case Group p: return this.Groups.Where(x => x.Name == Name).First() as T;
                    default: return new T();
                }
            }
            public List<T2> GetPartsByName<T,T2>(string Name) where T : class, new() where T2 : class, new()
            {
                switch (new T())
                {
                    case GeoPoint p:
                        {
                            var Names = this.Points.Where(x => x.Name == Name).First().PointsConnectedTo;
                            return Names.AsParallel().Select(x => this.GetByName<GeoPoint>(x) as T2).ToList();
                        }
                    case Polygon p:
                        {
                            var Names = this.Polygons.Where(x => x.Name == Name).First().MemberPoints;
                            return Names.AsParallel().Select(x => this.GetByName<GeoPoint>(x) as T2).ToList();
                        }
                    case Group g:
                        {
                            var Names = this.Groups.Where(x => x.Name == Name).First().MemberPolygons;
                            return Names.AsParallel().Select(x => this.GetByName<Polygon>(x) as T2).ToList();
                        }
                    default: return new List<T2>();
                }
            }
        }

        public class GeoPoint
        {
            public double X { get; set; }
            public double Y { get; set; }
            public string Name { get; set; } 
            public List<string> PointsConnectedTo { get; set; } //these are done as strings, so when a scene is saved it converts nicely to a JSON file.
        }

        public class Polygon
        {
            public List<string> MemberPoints { get; set; } //these are done as strings, so when a scene is saved it converts nicely to a JSON file.
            public double Y { get; set; }
            public double X { get; set;}
            public string Name { get; set; }
        }

        public class Group
        {
            public List<string> MemberPolygons { get; set; } //these are done as strings, so when a scene is saved it converts nicely to a JSON file.
            public double X { get; set; }
            public double Y { get; set; }
            public string Name { get; set; }
        }

    }
    public static class Functions
    {
        public static void RotateDegrees<T> (string Name, Templates.GeoScene Scene) where T : class, new()
        {
            switch (new T())
            {
                case Templates.GeoPoint p: break;
                case Templates.Polygon p:
                    {
                        var ConstituientPoints = Scene.GetPartsByName<Templates.Polygon,Templates.GeoPoint>(Name);
                        break;
                    }
                case Templates.Group g:
                    {
                        var ConstituientPolygons = Scene.GetPartsByName<Templates.Group, Templates.Polygon>(Name);
                        break;
                    }
                default: break;
            }
        }
        public static void Scale<T> (string Name, Templates.GeoScene Scene) where T : class, new()
        {
            switch (new T())
            {
                case Templates.GeoPoint p: break;
                case Templates.Polygon p:
                    {
                        var ConstituientPoints = Scene.GetPartsByName<Templates.Polygon, Templates.GeoPoint>(Name);
                        break;
                    }
                case Templates.Group g:
                    {
                        var ConstituientPolygons = Scene.GetPartsByName<Templates.Group, Templates.Polygon>(Name);
                        break;
                    }
                default: break;
            }
        }

        public static void Translate<T>(string Name, Templates.GeoScene Scene, int DestinationX) where T : class, new()
        {
            switch (new T())
            {
                case Templates.GeoPoint p:
                    {
                        var point= Scene.GetByName<Templates.GeoPoint>(Name);
                        //point.X;
                        //point.Y;
                        break;
                    }
                case Templates.Polygon p:
                    {
                        var ConstituientPoints = Scene.GetPartsByName<Templates.Polygon, Templates.GeoPoint>(Name);
                        break;
                    }
                case Templates.Group g:
                    {
                        var ConstituientPolygons = Scene.GetPartsByName<Templates.Group, Templates.Polygon>(Name);
                        break;
                    }
                default: break;
            }
        }

    }

}
