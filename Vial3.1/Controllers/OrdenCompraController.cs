using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using Vial3._1.Models;
using Vial3._1.Models.ViewModels;
using System.Data.Entity;
using PagedList;



using System.Drawing;
using ZXing;

using System.IO;
using Rotativa;
using System.Net;

namespace Vial3._1.Controllers
{
    public class OrdenCompraController : Controller
    {
        public ActionResult ValidarOC()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidarOC(OrdenDeCompraVM model)
        {
            if (ModelState.IsValidField("IdOrdenDeCompra"))
            {
                return RedirectToAction("ConfirmarOc", new {id = model.IdOrdenDeCompra});
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult ConfirmarOC(int id) {

            OrdenDeCompraVM OCvm = new OrdenDeCompraVM();
            using (vialcatEntities dc = new vialcatEntities())
            {
               
                //*******************************************************
                var OrdCom = dc.vial_ordendecompra
                                 .Include(d => d.vial_afiliados)
                                 .Include(d => d.vial_comercio)
                                 .Include(d=>d.vial_ordencomprapresentada)
                                 .Include(d=>d.vial_estadoordencompra)
                                 .FirstOrDefault(d => d.idOrdenDeCompra == id);

                if (OrdCom == null) {

                    OCvm.EstadoOC = 9;

                }
                else { 
                    OCvm.IdOrdenDeCompra = OrdCom.idOrdenDeCompra;               
                    OCvm.Importe = OrdCom.importe;
                    OCvm.FechaEmision = OrdCom.fecha_emision.Value;
                    OCvm.FechaVencimiento = OrdCom.fecha_vencimiento.Value;
                    OCvm.DatosAfiliado = OrdCom.vial_afiliados.apellido.ToUpper() + ", "+ OrdCom.vial_afiliados.nombre.ToUpper();
                    OCvm.Comercio = OrdCom.vial_comercio.id_comercio;
                    OCvm.RazonSocial = OrdCom.vial_comercio.razonsocial;
                    OCvm.EstadoOC = OrdCom.estado;
                    OCvm.EstadoDescripcion = OrdCom.vial_estadoordencompra.estadoordencompra;
                    
                } 
                //var OrdComPres = dc.vial_ordencomprapresentada
                //                .FirstOrDefault(d => d.idordendecompra == id);
                //if (OrdComPres == null)
                //{
                //    OCvm.EstadoOC = 1;
                //}
                //else
                //{
                //    OCvm.EstadoOC = 0;
                //}

                ViewBag.DatosOrden = OrdCom;

                return View(OCvm);

            }
        }
        //post 
        [HttpPost]
        public ActionResult ConfirmarOC(OrdenDeCompraVM model) {


            var id = model.IdOrdenDeCompra;

            using (vialcatEntities dc = new vialcatEntities()) {

                //agregar Orden de Compra Presentada
                vial_ordencomprapresentada Ocp = new vial_ordencomprapresentada();

                Ocp.idcomercio = model.Comercio;
                Ocp.idordendecompra = model.IdOrdenDeCompra;
                Ocp.fechapresentacion = DateTime.Today;
                Ocp.usuario = 1;
                Ocp.ip = Request.UserHostAddress;

                dc.vial_ordencomprapresentada.Add(Ocp);
                dc.SaveChanges();
                //ACTUALIZAR ORDEN DE COMPRA

                var query = (from a in dc.vial_ordendecompra
                             where a.idOrdenDeCompra == id
                             select a).FirstOrDefault();

                query.estado = 2;

                dc.SaveChanges();


            }
            return RedirectToAction ("ValidarOc");

        }
        
            // GET: OrdenCompra
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            List<AfiliadosComboVM> lstAfil;
            List<OrdenDeCompraVM> lstOC;
            

            using (vialcatEntities dc = new vialcatEntities())
            {
                lstAfil = (from d in dc.vial_afiliados
                           select new AfiliadosComboVM
                           {
                               IdAfiliado = d.idAfiliados,
                               Afiliado = d.apellido + ", " + d.nombre + "-" + d.documento.ToString()
                           }).ToList();


                lstOC = (from d in dc.vial_ordendecompra
                             .Include(d => d.vial_afiliados)
                             .Include(d => d.vial_comercio)
                         select new OrdenDeCompraVM
                         {
                             IdOrdenDeCompra = d.idOrdenDeCompra,
                             Afiliado = d.Afiliado,
                             FechaEmision =d.fecha_emision.Value,
                             FechaVencimiento =d.fecha_vencimiento.Value ,
                             Importe = d.importe,
                             Comercio = d.comercio,
                             Afiliados_lst = d.vial_afiliados,
                             Comercio_lst = d.vial_comercio
                         }).ToList();

            }

            switch (sortOrder)
            {
                case "name_desc":
                    lstOC = lstOC.OrderByDescending(s => s.Importe).ToList();
                    break;
                default:
                    lstOC = lstOC.OrderBy(s => s.IdOrdenDeCompra).ToList();
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                lstOC = lstOC.Where(d => d.IdOrdenDeCompra == int.Parse(searchString)).ToList();
                //|| d.Afiliado== int.Parse(searchString));
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
            return View(lstOC.ToPagedList(pageNumber, pageSize));


            //return View();
        }


        public ActionResult AgregarOC()
        {
            List<AfiliadosComboVM> lstAfil;
            List<ComercioComboVM> lstComercio;

            List<OrdenDeCompraVM> lstOrdComVM = null;

            using (vialcatEntities dc = new vialcatEntities())
            {
                lstAfil = (from d in dc.vial_afiliados
                           select new AfiliadosComboVM
                           {
                               IdAfiliado = d.idAfiliados,
                               Afiliado = d.apellido + ", " + d.nombre + "-" + d.documento.ToString()
                           }).ToList();


                lstComercio = (from d in dc.vial_comercio
                               select new ComercioComboVM
                               {
                                   IdComercio = d.id_comercio,
                                   Comercio = d.razonsocial
                               }).ToList();
                //parte nueva 
                lstOrdComVM = (from d in dc.vial_ordendecompra
                               .Include(d=>d.vial_comercio)
                                   select new OrdenDeCompraVM {

                                       IdOrdenDeCompra = d.idOrdenDeCompra,
                                       Afiliado = d.Afiliado,
                                       Importe = d.importe,
                                       Comercio = d.comercio,
                                       Comercio_lst=d.vial_comercio
                                       
                                       

                                   }).ToList();


            }

            List<SelectListItem> items = lstAfil.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Afiliado.ToString(),
                    Value = d.IdAfiliado.ToString(),
                    Selected = false
                };
            });
            ViewBag.afiliados = items;

            List<SelectListItem> itemsComercio = lstComercio.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Comercio.ToString(),
                    Value = d.IdComercio.ToString(),
                    Selected = false
                };
            });
            ViewBag.Comercio = itemsComercio;

            return View();

        }

        /*************************************************************************************/
        [HttpPost]
        public ActionResult AgregarOC(OrdenDeCompraVM model, string idAfiliado, string Comercio)
        {

            if (!ModelState.IsValid)
            {
                //SI EL MODELO NO ES VALIDO
                #region modeloNOvalido
                List<AfiliadosComboVM> lstAfil;
                List<ComercioComboVM> lstComercio;
                List<OrdenDeCompraVM> lstOrdComVM = null;

                using (vialcatEntities dc = new vialcatEntities())
                {
                    lstAfil = (from d in dc.vial_afiliados
                               select new AfiliadosComboVM
                               {
                                   IdAfiliado = d.idAfiliados,
                                   Afiliado = d.apellido + ", " + d.nombre + "-" + d.documento.ToString()
                               }).ToList();


                    lstComercio = (from d in dc.vial_comercio
                                   select new ComercioComboVM
                                   {
                                       IdComercio = d.id_comercio,
                                       Comercio = d.razonsocial
                                   }).ToList();
                    //parte nueva 
                    lstOrdComVM = (from d in dc.vial_ordendecompra
                                   select new OrdenDeCompraVM
                                   {
                                       IdOrdenDeCompra = d.idOrdenDeCompra,
                                       Afiliado = d.Afiliado,
                                       Importe = d.importe,
                                       Comercio = d.comercio,
                                       


                                   }).ToList();


                }

                List<SelectListItem> items = lstAfil.ConvertAll(d =>
                {
                    return new SelectListItem()
                    {
                        Text = d.Afiliado.ToString(),
                        Value = d.IdAfiliado.ToString(),
                        Selected = false
                    };
                });
                ViewBag.afiliados = items;

                List<SelectListItem> itemsComercio = lstComercio.ConvertAll(d =>
                {
                    return new SelectListItem()
                    {
                        Text = d.Comercio.ToString(),
                        Value = d.IdComercio.ToString(),
                        Selected = false
                    };
                });
                ViewBag.Comercio = itemsComercio;

                return View(model);


                #endregion modelovalid
            }

           

            else
            {
               
                using (vialcatEntities db = new vialcatEntities())
                {

                    vial_ordendecompra ocvm = new vial_ordendecompra();
                    ocvm.Afiliado = int.Parse(idAfiliado);
                    ocvm.importe = model.Importe;
                    ocvm.comercio = int.Parse(Comercio);
                    ocvm.fecha_emision =model.FechaEmision;
                    ocvm.fecha_vencimiento = model.FechaVencimiento;
                    ocvm.estado = 1;//1 corresponde a 'emitida'
                    db.vial_ordendecompra.Add(ocvm);
                    db.SaveChanges();
                }

                return RedirectToAction("AgregarOC");

            }

        }



        /******************************************************************/
        public ActionResult ModificarOc(int id, int idcomercio)
        {

            OrdenDeCompraVM OrdComVm = new OrdenDeCompraVM();

            List<SelectListItem> items = null;

            using (vialcatEntities dc = new vialcatEntities())
            {
                #region comercio
                var comercio = (from d in dc.vial_comercio
                                select new ComercioComboVM
                                {
                                    IdComercio = d.id_comercio,
                                    Comercio = d.razonsocial
                                }).ToList();


                items = new List<SelectListItem>();
                int numero = Convert.ToInt32(comercio[0].IdComercio.ToString());

                foreach (ComercioComboVM var in comercio)
                {
                    if (Convert.ToInt32(var.IdComercio.ToString()) == idcomercio)
                    {
                        items.Add(new SelectListItem() { Text = var.Comercio, Value = var.IdComercio.ToString(), Selected = true });
                    }
                    else {
                        items.Add(new SelectListItem() { Text = var.Comercio, Value = var.IdComercio.ToString(), Selected = false });
                    }
                }
                #endregion comercio

                //parte nueva ******************************************
                #region ObjetoOrdenDeCompra
                var OOrd_Com = dc.vial_ordendecompra
                                        .Include(c=>c.vial_afiliados)
                                        .FirstOrDefault(c => c.idOrdenDeCompra==id);
                OrdComVm.Comercio = OOrd_Com.comercio;
                OrdComVm.IdOrdenDeCompra = OOrd_Com.idOrdenDeCompra;
                OrdComVm.Afiliado = OOrd_Com.Afiliado;
                OrdComVm.Importe = OOrd_Com.importe;

                OrdComVm.FechaEmision = OOrd_Com.fecha_emision.Value;

                OrdComVm.DatosAfiliado = OOrd_Com.vial_afiliados.apellido.ToUpper() + ", " + OOrd_Com.vial_afiliados.nombre.ToUpper();

                OrdComVm.FechaVencimiento = OOrd_Com.fecha_vencimiento.Value;
                #endregion ObjetoOrdenDeCompra
            }

            ViewBag.Comercio = items;

            return View(OrdComVm);
        }
        /*********************************************************************/



        [HttpPost]
        public ActionResult ModificarOc(OrdenDeCompraVM model)
        {
            List<SelectListItem> items = null;

            OrdenDeCompraVM OrdComVm = new OrdenDeCompraVM();

            if (!ModelState.IsValid)
            {
                //***********************************************************
                using (vialcatEntities dc = new vialcatEntities())
                {
                    /*
                    var objOrdCom = dc.vial_ordendecompra.Find(model.IdOrdenDeCompra);

                    objOrdCom.importe = model.Importe;
                    objOrdCom.comercio = model.Comercio;
                    objOrdCom.fecha_emision = model.FechaEmision;
                    objOrdCom.fecha_vencimiento = model.FechaVencimiento;

                    //dc.Entry(objOrdCom).State = EntityState.Modified;

                    //dc.SaveChanges();
                    */
                    var comercio = (from d in dc.vial_comercio
                                    select new ComercioComboVM
                                    {
                                        IdComercio = d.id_comercio,
                                        Comercio = d.razonsocial
                                    }).ToList();

                    items = new List<SelectListItem>();
                    int numero = Convert.ToInt32(comercio[0].IdComercio.ToString());

                    foreach (ComercioComboVM var in comercio)
                    {
                        if (Convert.ToInt32(var.IdComercio.ToString()) == numero)
                        {
                            items.Add(new SelectListItem() { Text = var.Comercio, Value = var.IdComercio.ToString(), Selected = true });
                        }
                        else
                        {
                            items.Add(new SelectListItem() { Text = var.Comercio, Value = var.IdComercio.ToString(), Selected = false });
                        }
                    }
                    /*
                    var OOrd_Com = dc.vial_ordendecompra
                                       .Include(c => c.vial_afiliados)
                                       .FirstOrDefault(c => c.idOrdenDeCompra == model.IdOrdenDeCompra);
                    OrdComVm.Comercio = OOrd_Com.comercio;
                    OrdComVm.IdOrdenDeCompra = OOrd_Com.idOrdenDeCompra;
                    OrdComVm.Afiliado = OOrd_Com.Afiliado;
                    OrdComVm.Importe = OOrd_Com.importe;

                    OrdComVm.FechaEmision = OOrd_Com.fecha_emision.Value;

                    OrdComVm.DatosAfiliado = OOrd_Com.vial_afiliados.apellido.ToUpper() + ", " + OOrd_Com.vial_afiliados.nombre.ToUpper();

                    OrdComVm.FechaVencimiento = OOrd_Com.fecha_vencimiento.Value;
                   

                    */
                    ViewBag.Comercio = items;
                    return View (model);        
                }
                
                
                
                //***********************************************************
            }
            else {

                //try
                //{
                using (vialcatEntities dc = new vialcatEntities())
                    {

                    var objOrdCom = dc.vial_ordendecompra.Find(model.IdOrdenDeCompra);

                        objOrdCom.importe = model.Importe;
                        objOrdCom.comercio = model.Comercio;
                        objOrdCom.fecha_emision = model.FechaEmision;
                        objOrdCom.fecha_vencimiento = model.FechaVencimiento;

                        dc.Entry(objOrdCom).State = EntityState.Modified;
                        dc.SaveChanges();

                    var comercio = (from d in dc.vial_comercio
                                    select new ComercioComboVM
                                    {
                                        IdComercio = d.id_comercio,
                                        Comercio = d.razonsocial
                                    }).ToList();

                    items = new List<SelectListItem>();
                    int numero = Convert.ToInt32(comercio[0].IdComercio.ToString());

                    foreach (ComercioComboVM var in comercio)
                    {
                        if (Convert.ToInt32(var.IdComercio.ToString()) == numero)
                        {
                            items.Add(new SelectListItem() { Text = var.Comercio, Value = var.IdComercio.ToString(), Selected = true });
                        }
                        else
                        {
                            items.Add(new SelectListItem() { Text = var.Comercio, Value = var.IdComercio.ToString(), Selected = false });
                        }
                    }
                   
                    }

                return RedirectToAction("index");
                //}
                //catch (Exception ex) {


                //}

            }



        }



        /*/****************************************************************/
        public ActionResult VerOC(int id) {

            OrdenDeCompraVM OrdComVm = new OrdenDeCompraVM();

            using (vialcatEntities dc = new vialcatEntities())
            {
                //parte nueva ******************************************
                #region ObjetoOrdenDeCompra
                var OOrd_Com = dc.vial_ordendecompra
                                        .Include(c => c.vial_afiliados)
                                        .FirstOrDefault(c => c.idOrdenDeCompra == id);
                OrdComVm.Comercio = OOrd_Com.comercio;
                OrdComVm.IdOrdenDeCompra = OOrd_Com.idOrdenDeCompra;
                OrdComVm.Afiliado = OOrd_Com.Afiliado;
                OrdComVm.Importe = OOrd_Com.importe;

                OrdComVm.FechaEmision = OOrd_Com.fecha_emision.Value;

                OrdComVm.DatosAfiliado = OOrd_Com.vial_afiliados.apellido.ToUpper() + ", " + OOrd_Com.vial_afiliados.nombre.ToUpper();

                OrdComVm.FechaVencimiento = OOrd_Com.fecha_vencimiento.Value;

                #endregion ObjetoOrdenDeCompra
            }


            using (var ms = new MemoryStream())
            {
                string userid = id.ToString();
                Image img = null;

                var writer = new ZXing.BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
                writer.Options.Height = 80;
                writer.Options.Width = 280;
                writer.Options.PureBarcode = true;
                img = writer.Write(userid);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                //return File(ms.ToArray(), "image/jpeg");
                ViewBag.Codigo = "data:image/jpeg;base64," + Convert.ToBase64String(ms.ToArray(), 0, ms.ToArray().Length);
            }

           
            return View(OrdComVm);
            

        }

        /*/****************************************************************/
        public ActionResult ImprimirOc(int id) {

            return new ActionAsPdf("VerOc", new {id=id}) { FileName="oc.pdf"};

        }

    }
}