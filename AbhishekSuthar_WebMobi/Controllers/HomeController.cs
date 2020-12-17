using AbhishekSuthar_WebMobi.Models;
using AbhishekSuthar_WebMobi.Views.ViewModelClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbhishekSuthar_WebMobi.Controllers
{
    public class HomeController : Controller
    {
        private readonly CDbContext _context = null;

        public HomeController()
        {
            _context = new CDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult InsertEmployee(Employee obj)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;

                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string Fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            Fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            Fname = file.FileName;
                        }

                        var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads";
                        Directory.CreateDirectory(uploadRootFolderInput);
                        var directoryFullPathInput = uploadRootFolderInput;
                        Fname = Path.Combine(directoryFullPathInput, Fname);
                        file.SaveAs(Fname);

                        obj.Image = file.FileName;
                    }

                    _context.employees.Add(obj);
                    _context.SaveChanges();
                    return Json("Success", JsonRequestBehavior.AllowGet);


                }
                catch (Exception ex)
                {

                    return Json("error occured .Error detail: ", ex.Message);
                }
            }
            else
            {
                return Json("No file selected");
            }


        }


        public JsonResult GetDepartmentList()
        {
            var data = _context.departments.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DisplayListData()
        {
            JsonResult result = new JsonResult();
            try
            {
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);



                //List<Employee> data = _context.employees.ToList();
                List<EmployeeViewModel> data = new List<EmployeeViewModel>();
                data = (from e1 in _context.employees
                        join d1 in _context.departments on e1.DepartmentId equals d1.DepartmentId
                        select new EmployeeViewModel
                        {
                          

                            EmpId = e1.EmpId,
                            FirstName = e1.FirstName,
                            LastName = e1.LastName,
                            Image = e1.Image,
                            Gender = e1.Gender,
                            Dob = e1.Dob,
                            DepartmentName = d1.DepartmentName

                        }).ToList();




                int totalRecords = data.Count;
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(p => p.EmpId.ToString().ToLower().Contains(search.ToLower()) ||
                        p.FirstName.ToString().Contains(search.ToLower()) ||
                        p.LastName.ToString().Contains(search.ToLower()) ||
                        p.Gender.ToString().Contains(search.ToLower()) ||
                        p.DepartmentName.ToString().Contains(search.ToLower()) ||

                        p.Dob.ToString().Contains(search.ToLower()) ||
                        p.Image.ToString().Contains(search.ToLower())
                     ).ToList();
                }
                data = SortTableData(order, orderDir, data);
                int recFilter = data.Count;
                data = data.Skip(startRec).Take(pageSize).ToList();
                var modifiedData = data.Select(d =>
                    new
                    {
                        d.EmpId,
                        d.FirstName,
                        d.LastName,
                        d.Gender,
                        d.Dob,
                        d.DepartmentName,
                        d.Image
                    }
                    );
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = modifiedData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return result;
        }
        private List<EmployeeViewModel> SortTableData(string order, string orderDir, List<EmployeeViewModel> data)
        {
            List<EmployeeViewModel> lst = new List<EmployeeViewModel>();
            try
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EmpId).ToList()
                                                                                                 : data.OrderBy(p => p.EmpId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FirstName).ToList()
                                                                                                 : data.OrderBy(p => p.FirstName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastName).ToList()
                                                                                                 : data.OrderBy(p => p.LastName).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Gender).ToList()
                                                                                                 : data.OrderBy(p => p.Gender).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Dob).ToList()
                                                                                                   : data.OrderBy(p => p.Dob).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DepartmentName).ToList()
                                                                                                   : data.OrderBy(p => p.DepartmentName).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EmpId).ToList()
                                                                                                 : data.OrderBy(p => p.EmpId).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return lst;
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var data = _context.employees.Find(id);
            _context.employees.Remove(data);
            _context.SaveChanges();

            var massage = "Delete Sucessfully";
            return Json(massage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetByID(int id)
        {
            var data = _context.employees.Find(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Update(Employee obj)
        {

            var empdata = _context.employees.FirstOrDefault(x => x.EmpId == obj.EmpId);
            if (empdata != null)
            {

                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads";
                        Directory.CreateDirectory(uploadRootFolderInput);
                        var directoryFullPathInput = uploadRootFolderInput;
                        fname = Path.Combine(directoryFullPathInput, fname);
                        file.SaveAs(fname);
                        obj.Image = file.FileName;
                    }

                }

                empdata.FirstName = obj.FirstName;
                empdata.LastName = obj.LastName;
                empdata.Gender = obj.Gender;
                empdata.DepartmentId = obj.DepartmentId;
                empdata.Dob = obj.Dob;
                _context.employees.Add(empdata);
                _context.SaveChanges();


            }
            return Json(empdata, JsonRequestBehavior.AllowGet);
        }

    }
}