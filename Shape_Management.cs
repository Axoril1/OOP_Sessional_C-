using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

enum shape_type{ 
    circle, rectangle, cube 
}

abstract class shape{
    public int id;
    public shape_type type;
    public abstract double area();
    public abstract double perimeter();
    public abstract double volume();
    public abstract string dimensions_text();
    public string type_text(){ 
        return type.ToString().ToLower(); 
    }
}

class circle : shape{
    public double diameter;
    public circle(int id, double d){
        this.id = id;
        this.type = shape_type.circle;
        this.diameter = d;
    }
    public override double area(){ 
        double r = diameter / 2.0; 
        return Math.PI * r * r; 
    }
    public override double perimeter(){
        return Math.PI * diameter;
    }
    public override double volume(){ 
        return 0.0; 
    }
    public override string dimensions_text(){
        return "d=" + diameter.ToString("0.##", CultureInfo.InvariantCulture);
    }
}

class rectangle : shape{
    public double height;
    public double width;
    public rectangle(int id, double h, double w){
        this.id = id;
        this.type = shape_type.rectangle;
        this.height = h;
        this.width = w;
    }
    public override double area(){
        return height * width; 
    }
    public override double perimeter(){ 
        return 2.0 * (height + width); 
    }
    public override double volume(){ 
        return 0.0; 
    }
    public override string dimensions_text(){
        return "h=" + height.ToString("0.##", CultureInfo.InvariantCulture) +
               " w=" + width.ToString("0.##", CultureInfo.InvariantCulture);
    }
}

class cube : shape{
    public double height;
    public double width;
    public double depth;
    public cube(int id, double h, double w, double d){
        this.id = id;
        this.type = shape_type.cube;
        this.height = h;
        this.width = w;
        this.depth = d;
    }
    public override double area(){ 
        return 2.0 * (height * width + width * depth + depth * height); 
    }
    public override double perimeter(){ 
        return 0.0; 
    }
    public override double volume(){ 
        return height * width * depth; 
    }
    public override string dimensions_text(){
        return "h=" + height.ToString("0.##", CultureInfo.InvariantCulture) +
               " w=" + width.ToString("0.##", CultureInfo.InvariantCulture) +
               " d=" + depth.ToString("0.##", CultureInfo.InvariantCulture);
    }
}

class program{
    static int read_int_in_range(string prompt, int min, int max){
        while(true){
            Console.Write(prompt);
            var line = Console.ReadLine();
            if(int.TryParse(line, out int val)){
                if (val >= min && val <= max) return val;
            }
            Console.WriteLine("please enter a number between " + min + " and " + max + ".");
        }
    }

    static double read_positive_double(string prompt){
        while(true){
            Console.Write(prompt);
            var line = Console.ReadLine();
            if(double.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out double val)){
                if (val > 0.0) return val;
            }
            Console.WriteLine("please enter a value > 0.");
        }
    }

    static bool read_yes_no(string prompt){
        while(true){
            Console.Write(prompt);
            var line = Console.ReadLine();
            if(string.IsNullOrWhiteSpace(line)) continue;
            char c = char.ToLowerInvariant(line.Trim()[0]);
            if(c == 'y') return true;
            if(c == 'n') return false;
            Console.WriteLine("please enter y or n.");
        }
    }

    static string read_line(string prompt){
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }

    static void add_circle(List<shape> shapes, ref int next_id){
        double d = read_positive_double("enter diameter (>0): ");
        var s = new circle(next_id++, d);
        shapes.Add(s);
        Console.WriteLine("circle added.");
    }

    static void add_rectangle(List<shape> shapes, ref int next_id){
        double h = read_positive_double("enter height (>0): ");
        double w = read_positive_double("enter width  (>0): ");
        var s = new rectangle(next_id++, h, w);
        shapes.Add(s);
        Console.WriteLine("rectangle added.");
    }

    static void add_cube(List<shape> shapes, ref int next_id){
        double h = read_positive_double("enter height (>0): ");
        double w = read_positive_double("enter width  (>0): ");
        double d = read_positive_double("enter depth  (>0): ");
        var s = new cube(next_id++, h, w, d);
        shapes.Add(s);
        Console.WriteLine("cube added.");
    }

    static int find_index_by_id(List<shape> shapes, int id){
        for(int i = 0; i < shapes.Count; i++){
            if(shapes[i].id == id) return i;
        }
        return -1;
    }

    static void edit_shape(List<shape> shapes){
        if(shapes.Count == 0){
            Console.WriteLine("No shapes to edit.");
            return;
        }
        int id = read_int_in_range("Enter the id to edit: ", 1, int.MaxValue);
        int idx = find_index_by_id(shapes, id);
        if(idx < 0){
            Console.WriteLine("Id not found.");
            return;
        }
        var s = shapes[idx];
        Console.WriteLine("Editing [" + s.id + "] " + s.type_text());
        if(s is circle cc){
            cc.diameter = read_positive_double("New diameter (>0): ");
        }
        else if(s is rectangle rr){
            rr.height = read_positive_double("New height (>0): ");
            rr.width = read_positive_double("New width  (>0): ");
        }
        else if(s is cube uu){
            uu.height = read_positive_double("New height (>0): ");
            uu.width = read_positive_double("New width  (>0): ");
            uu.depth = read_positive_double("New depth  (>0): ");
        }
        Console.WriteLine("Updated.");
    }

    static void remove_shape(List<shape> shapes){
        if(shapes.Count == 0){
            Console.WriteLine("no shapes to delete.");
            return;
        }
        int id = read_int_in_range("enter the id to delete: ", 1, int.MaxValue);
        int idx = find_index_by_id(shapes, id);
        if(idx < 0){
            Console.WriteLine("id not found.");
            return;
        }
        Console.Write("you are about to delete id " + id + ". ");
        if(!read_yes_no("are you sure? (y/n): ")){
            Console.WriteLine("canceled.");
            return;
        }
        shapes.RemoveAt(idx);
        Console.WriteLine("deleted.");
    }

    static void print_shape_row(shape s){
        string dims = s.dimensions_text();
        string a = s.area().ToString("0.##", CultureInfo.InvariantCulture);
        string p = s.type == shape_type.cube ? "-" : s.perimeter().ToString("0.##", CultureInfo.InvariantCulture);
        string v = s.type == shape_type.cube ? s.volume().ToString("0.##", CultureInfo.InvariantCulture) : "-";
        Console.WriteLine("{0,-4} {1,-10} {2,-24} {3,14} {4,12} {5,12}", s.id, s.type_text(), dims, a, p, v);
    }

    static void list_shapes(List<shape> shapes){
        if (shapes.Count == 0){
            Console.WriteLine("No shapes yet.");
            return;
        }

        int filter = read_int_in_range("Filter (0=all, 1=circle, 2=rectangle, 3=cube): ", 0, 3);
        int sort = read_int_in_range("Sort by (0=none, 1=id, 2=area): ", 0, 2);
        int asc = sort == 0 ? 1 : read_int_in_range("Order (1=asc, 2=desc): ", 1, 2);

        var view = new List<shape>();
        foreach(var s in shapes){
            if(filter == 0 ||
                (filter == 1 && s.type == shape_type.circle) ||
                (filter == 2 && s.type == shape_type.rectangle) ||
                (filter == 3 && s.type == shape_type.cube)){
                view.Add(s);
            }
        }

        if(sort == 1){
            view.Sort((a, b) => asc == 1 ? a.id.CompareTo(b.id) : b.id.CompareTo(a.id));
        }
        else if (sort == 2){
            view.Sort((a, b) =>{
                int cmp = a.area().CompareTo(b.area());
                return asc == 1 ? cmp : -cmp;
            });
        }

        Console.WriteLine();
        Console.WriteLine("{0,-4} {1,-10} {2,-24} {3,14} {4,12} {5,12}", "id", "type", "dimensions", "area/surface", "perimeter", "volume");
        Console.WriteLine("-------------------------------------------------------------------------------");
        foreach (var s in view) print_shape_row(s);
        Console.WriteLine();
    }

    static void show_stats(List<shape> shapes){
        int count_circle = 0, count_rectangle = 0, count_cube = 0;
        double area_total = 0.0, area_circle = 0.0, area_rectangle = 0.0, area_cube_surface = 0.0, volume_cube_total = 0.0;
        int max_id = -1;
        double max_area = -1.0;
        shape max_shape = null;

        foreach(var s in shapes){
            double a = s.area();
            area_total += a;
            if(a > max_area){
                max_area = a;
                max_shape = s;
                max_id = s.id;
            }
            if(s.type == shape_type.circle){
                count_circle++;
                area_circle += a;
            }
            else if(s.type == shape_type.rectangle){
                count_rectangle++;
                area_rectangle += a;
            }
            else if(s.type == shape_type.cube){
                count_cube++;
                area_cube_surface += a;
                volume_cube_total += s.volume();
            }
        }

        Console.WriteLine("Total shapes: " + shapes.Count);
        Console.WriteLine("Circles: " + count_circle);
        Console.WriteLine("Rectangles: " + count_rectangle);
        Console.WriteLine("Cubes: " + count_cube);
        Console.WriteLine();
        Console.WriteLine("Area/Surface totals:");
        Console.WriteLine("  Circles:    " + area_circle.ToString("0.##", CultureInfo.InvariantCulture));
        Console.WriteLine("  Rectangles: " + area_rectangle.ToString("0.##", CultureInfo.InvariantCulture));
        Console.WriteLine("  Cubes(sa):  " + area_cube_surface.ToString("0.##", CultureInfo.InvariantCulture));
        Console.WriteLine("Grand total area/surface: " + area_total.ToString("0.##", CultureInfo.InvariantCulture));

        if(area_total > 0.0){
            Console.WriteLine();
            Console.WriteLine("percentages by area/surface:");
            Console.WriteLine("  circles:    " + ((area_circle / area_total) * 100.0).ToString("0.##", CultureInfo.InvariantCulture) + "%");
            Console.WriteLine("  rectangles: " + ((area_rectangle / area_total) * 100.0).ToString("0.##", CultureInfo.InvariantCulture) + "%");
            Console.WriteLine("  cubes:      " + ((area_cube_surface / area_total) * 100.0).ToString("0.##", CultureInfo.InvariantCulture) + "%");
        }

        Console.WriteLine();
        Console.WriteLine("total cube volume: " + volume_cube_total.ToString("0.##", CultureInfo.InvariantCulture));

        if(max_id != -1 && max_shape != null){
            Console.WriteLine();
            Console.WriteLine("largest by area/surface: id " + max_id + " (" + max_shape.type_text() + ") with " + max_area.ToString("0.##", CultureInfo.InvariantCulture));
        }
    }

    static bool save_to_file(string filename, List<shape> shapes){
        try{
            using (var sw = new StreamWriter(filename)){
                foreach(var s in shapes){
                    if (s is circle cc){
                        sw.WriteLine("c {0} {1}", s.id, cc.diameter.ToString("R", CultureInfo.InvariantCulture));
                    }
                    else if(s is rectangle rr){
                        sw.WriteLine("r {0} {1} {2}", s.id,
                            rr.height.ToString("R", CultureInfo.InvariantCulture),
                            rr.width.ToString("R", CultureInfo.InvariantCulture));
                    }
                    else if(s is cube uu){
                        sw.WriteLine("u {0} {1} {2} {3}", s.id,
                            uu.height.ToString("R", CultureInfo.InvariantCulture),
                            uu.width.ToString("R", CultureInfo.InvariantCulture),
                            uu.depth.ToString("R", CultureInfo.InvariantCulture));
                    }
                }
            }
            return true;
        }
        catch{
            return false;
        }
    }

    static bool load_from_file(string filename, List<shape> shapes, ref int next_id){
        try{
            if(!File.Exists(filename)) return false;
            var lines = File.ReadAllLines(filename);
            var new_list = new List<shape>();
            int max_id = 0;

            foreach (var line in lines){
                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;
                string t = parts[0].ToLowerInvariant();
                if(t == "c" && parts.Length >= 3){
                    int id = int.Parse(parts[1], CultureInfo.InvariantCulture);
                    double d = double.Parse(parts[2], CultureInfo.InvariantCulture);
                    new_list.Add(new circle(id, d));
                    if(id > max_id) max_id = id;
                }
                else if(t == "r" && parts.Length >= 4){
                    int id = int.Parse(parts[1], CultureInfo.InvariantCulture);
                    double h = double.Parse(parts[2], CultureInfo.InvariantCulture);
                    double w = double.Parse(parts[3], CultureInfo.InvariantCulture);
                    new_list.Add(new rectangle(id, h, w));
                    if(id > max_id) max_id = id;
                }
                else if(t == "u" && parts.Length >= 5){
                    int id = int.Parse(parts[1], CultureInfo.InvariantCulture);
                    double h = double.Parse(parts[2], CultureInfo.InvariantCulture);
                    double w = double.Parse(parts[3], CultureInfo.InvariantCulture);
                    double d = double.Parse(parts[4], CultureInfo.InvariantCulture);
                    new_list.Add(new cube(id, h, w, d));
                    if(id > max_id) max_id = id;
                }
            }
            shapes.Clear();
            shapes.AddRange(new_list);
            next_id = max_id + 1;
            return true;
        }
        catch{
            return false;
        }
    }

    static void Main(){
        var shapes = new List<shape>();
        int next_id = 1;

        while(true){
            Console.WriteLine();
            Console.WriteLine("================== Shape manager ==================");
            Console.WriteLine("1. Add a circle");
            Console.WriteLine("2. Add a rectangle");
            Console.WriteLine("3. Add a cube");
            Console.WriteLine("4. List items");
            Console.WriteLine("5. Get statistics");
            Console.WriteLine("6. Edit an item");
            Console.WriteLine("7. Remove an item");
            Console.WriteLine("8. Save to file");
            Console.WriteLine("9. Load from file");
            Console.WriteLine("0. Exit");
            Console.WriteLine("===================================================");
            int choice = read_int_in_range("Choose an option: ", 0, 9);

            if(choice == 1) add_circle(shapes, ref next_id);
            else if(choice == 2) add_rectangle(shapes, ref next_id);
            else if(choice == 3) add_cube(shapes, ref next_id);
            else if(choice == 4) list_shapes(shapes);
            else if(choice == 5) show_stats(shapes);
            else if(choice == 6) edit_shape(shapes);
            else if(choice == 7) remove_shape(shapes);
            else if(choice == 8){
                string fname = read_line("file to save (default: shapes.txt): ");
                if(string.IsNullOrWhiteSpace(fname)) fname = "shapes.txt";
                if(save_to_file(fname, shapes)) Console.WriteLine("saved.");
                else Console.WriteLine("failed to save.");
            }
            else if(choice == 9){
                string fname = read_line("file to load (default: shapes.txt): ");
                if(string.IsNullOrWhiteSpace(fname)) fname = "shapes.txt";
                if(load_from_file(fname, shapes, ref next_id)) Console.WriteLine("loaded.");
                else Console.WriteLine("failed to load.");
            }
            else if(choice == 0){
                Console.WriteLine("goodbye.");
                return;
            }
        }
    }
}
