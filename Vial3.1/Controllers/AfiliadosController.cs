using Vial3._1.Models.ViewModels;
using Vial3._1.Models;

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using PagedList;


namespace Vial3._1.Controllers
{
    public class AfiliadosController : Controller
    {
        private vialcatEntities dc = new vialcatEntities();

        // GET: Afiliado
        [HttpGet]
        public ActionResult Agregar()
        {
             return View();
        }

        [HttpPost]
        public ActionResult Agregar(AfiliadoVM model)
        {
            try {
                using (vialcatEntities db = new vialcatEntities())
                {

                    vial_afiliados objafil = new vial_afiliados();

                    objafil.apellido = model.Apellido;
                    objafil.nombre = model.Nombre;
                    objafil.documento = model.Documento;
                    objafil.domicilio = model.Domicilio;
                    objafil.telefono = model.Telefono;

                    db.vial_afiliados.Add(objafil);
                    db.SaveChanges();

                    ViewBag.Message = "Registro Insertado";

                    return View();
                }
            }
            catch { 
                return View();
            }
        }


        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page) {

            vialcatEntities dc = new vialcatEntities();

            var afiliados = from d in dc.vial_afiliados.Where(d=>d.estado_afiliacion==1)
                           select d;
            
            switch (sortOrder)
            {
                case "name_desc":
                    afiliados = afiliados.OrderBy(s => s.apellido);
                    break;
                default:
                    afiliados = afiliados.OrderBy(s => s.apellido);
                    break;
            }


            if (!String.IsNullOrEmpty(searchString))
            {
                afiliados = afiliados.Where(d => d.apellido.Contains(searchString)
                                       || d.nombre.Contains(searchString));
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(afiliados.ToPagedList(pageNumber, pageSize));
                       



            //return View(afiliados);

        }

        public ActionResult Eliminar(int id) {

            var afiliado = dc.vial_afiliados.Find(id);
            return View(afiliado);


        }
    }
}