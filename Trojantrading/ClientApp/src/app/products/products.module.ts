import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material';
import { productsRoutingModule } from './products-routing.module.';
import { NewProductComponent } from './new-product/new-product.component';
import { PromotionComponent } from './promotion/promotion.component';
import { SoldOutComponent } from './sold-out/sold-out.component';
import { ProductsComponent } from './products.component';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    productsRoutingModule
  ],
  declarations: [
    ProductsComponent,
    NewProductComponent,
    PromotionComponent,
    SoldOutComponent
  ],
  providers: []
})
export class ProductsModule {}
