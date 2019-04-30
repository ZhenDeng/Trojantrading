import { ProductService } from './../services/product.service';
import { Product } from './../models/Product';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { Menu } from '../models/menu';
import { ShareService } from '../services/share.service';
import { NavbarService } from '../services/navbar.service';
import { ApiResponse } from '../models/ApiResponse';
import { EditProductComponent } from '../popup-collection/edit-product/edit-product.component';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  products: Product[] = [];

  filteredProducts: Product[] = [];

  dataSource = new MatTableDataSource();

  viewType: string;

  title: string = '';

  displayedColumns: string[];

  role: string;

  navLinks: Menu[] = [
    {
      path: '/home',
      label: 'All Products',
      id: 'allProducts'
    },
    {
      path: '/productsview/new',
      label: 'New Product',
      id: 'newProduct'
    },
    {
      path: '/productsview/promotion',
      label: 'Promotions',
      id: 'promotions'
    },
    {
      path: '/productsview/soldout',
      label: 'Sold Out',
      id: 'soldout'
    },
  ];

  constructor(
    private router: Router,
    public nav: NavbarService,
    private activatedRouter: ActivatedRoute,
    private productService: ProductService,
    private shareService: ShareService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.nav.show();
    this.nav.showTab();
    this.role = this.shareService.readCookie("role");
    if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "admin") {
      this.displayedColumns = ['name', 'category', 'originalPrice', 'agentPrice', 'resellerPrice', 'button']
    }
    else if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "agent") {
      this.displayedColumns = ['name', 'category', 'originalPrice', 'agentPrice', 'qty', 'button']
    }
    else if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "reseller") {
      this.displayedColumns = ['name', 'category', 'originalPrice', 'resellerPrice', 'qty', 'button']
    } else {
      this.displayedColumns = ['name', 'category', 'originalPrice', 'qty', 'button']
    }
    this.activatedRouter.paramMap.subscribe(param => {
      this.viewType = param.get('type');
      const type = this.viewType.toLowerCase();
      if (type === 'new') {
        this.title = 'New Products';
      } else if (type.includes('promotion')) {
        this.title = 'Promotions';
      } else {
        this.title = 'Sold Out';
      }

      if (!this.products.length) {
        this.getProducts(type);
      } else {
        this.filterProducts(type, this.products);
      }

    });
  }

  getProducts(type: string) {
    this.productService.getAllProducts().subscribe((value: Product[]) => {
      this.products = value;
      this.filterProducts(type, value);
    });
  }

  filterProducts(type: string, value: Product[]) {
    this.filteredProducts = value.filter(x => x.status.toLowerCase().includes(type));
    this.filteredProducts.forEach(product => product.quantity = 1);
    this.dataSource = new MatTableDataSource(this.filteredProducts);
  }

  applyFilter(value: string) {
    value = value.trim();
    value = value.toLowerCase();
    this.dataSource.filter = value;
  }

  _keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    let inputChar = String.fromCharCode(event.charCode);

    if (!pattern.test(inputChar)) {
      // invalid character, prevent input
      event.preventDefault();
    }
  }

  editProduct(product: Product): void {
    const dialogRef = this.dialog.open(EditProductComponent, {
      width: '700px',
      data: { product: product, categorys: this.productService.categoryList }
    });

    dialogRef.afterClosed().subscribe(result => {
      product.name = result.name;
      product.category = result.category;
      product.agentPrice = result.agentPrice;
      product.originalPrice = result.originalPrice;
      product.resellerPrice = result.resellerPrice;
      this.productService.UpdateProduct(product).subscribe((res: ApiResponse) => {
        if (res.status == "success") {
          this.shareService.showSuccess("#" + product.id, res.message, "right");
          setTimeout(() => {
            this.activatedRouter.paramMap.subscribe(param => {
              this.viewType = param.get('type');
              const type = this.viewType.toLowerCase();
              if (type === 'new') {
                this.title = 'New Products';
              } else if (type.includes('promotion')) {
                this.title = 'Promotions';
              } else {
                this.title = 'Sold Out';
              }

              if (!this.products.length) {
                this.getProducts(type);
              } else {
                this.filterProducts(type, this.products);
              }

            });
          }, 2000);
        } else {
          this.shareService.showError("#" + product.id, res.message, "right");
        }
      },
        (error: any) => {
          console.info(error);
        });
    });
  }

  ngOnDestroy() {

  }

}
