import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatTabsModule } from '@angular/material';
import { productsRoutingModule } from './products-routing.module.';
import { NewProductComponent } from './new-product/new-product.component';
import { PromotionComponent } from './promotion/promotion.component';
import { SoldOutComponent } from './sold-out/sold-out.component';
import { ProductsComponent } from './products.component';
import { ProductService } from '../services/product.service';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    productsRoutingModule,
    MatTableModule,
    MatInputModule,
    MatTabsModule,
  ],
  declarations: [
    ProductsComponent,
    NewProductComponent,
    PromotionComponent,
    SoldOutComponent
  ],
  providers: [
    ProductService
  ]
})
export class ProductsModule {}
