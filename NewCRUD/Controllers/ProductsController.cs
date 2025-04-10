using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewCRUD.Models;
using NewCRUD.Services;

namespace NewCRUD.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _dbcontext = db;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _dbcontext.Products.ToList();

            return View(objProductList);
        }

        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The Image File Is Required");
                return View(productDto);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(productDto.ImageFile.FileName);
            string imagesFolder = Path.Combine(environment.WebRootPath, "Products");

            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            string imageFullPath = Path.Combine(imagesFolder, newFileName);

            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }

            if (ModelState.IsValid)
            {

                var product = new Product
                {
                    Name = productDto.Name,
                    Brand = productDto.Brand,
                    Category = productDto.Category,
                    Price = productDto.Price,
                    Description = productDto.Description,
                    ImageFileName = newFileName
                };

                _dbcontext.Products.Add(product);
                _dbcontext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productDto);
        }

        public IActionResult EditProduct(int? Id)
        {
            var product = _dbcontext.Products.Find(Id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            var productDto = new ProductDto
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,

            };
            ViewData["productId"] = product.Id;
            ViewData["imageFileName"] = product.ImageFileName;
            return View(productDto);
        }
        [HttpPost]
        public IActionResult EditProduct(int? Id, ProductDto productDto)
        {
            var product = _dbcontext.Products.Find(Id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                ViewData["productId"] = product.Id;
                ViewData["imageFileName"] = product.ImageFileName;
                return View(productDto);
            }
            string newFileName = product.ImageFileName;
            if (productDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetFileName(productDto.ImageFile.FileName);

                string imageFullPath = environment.WebRootPath + "/Products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);
                }
                string oldImageFullPath = environment.WebRootPath + "/Products/" + product.ImageFileName;
                //System.IO.File.Delete(oldImageFullPath);
                if (System.IO.File.Exists(oldImageFullPath))
                {
                    System.IO.File.Delete(oldImageFullPath);
                }

            }
            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Category = productDto.Category;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.ImageFileName = newFileName;
            _dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
            var product = _dbcontext.Products.Find(Id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            string imageFullPath = environment.WebRootPath + "/Products" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);
            _dbcontext.Products.Remove(product);
            _dbcontext.SaveChanges(true);
            return RedirectToAction("Index"); 
        }
    }
}
