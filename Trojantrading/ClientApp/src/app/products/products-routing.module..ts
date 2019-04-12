import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NewProductComponent } from './new-product/new-product.component';
import { PromotionComponent } from './promotion/promotion.component';
import { SoldOutComponent } from './sold-out/sold-out.component';
// import { ProductsComponent } from './products.component';
import { HomeComponent } from '../home/home.component';

const routes: Routes = [
    {
        path: 'product',
        component: HomeComponent,
        children: [
            {
                path: 'new',
                component: NewProductComponent,
            },
            {
                path: 'promotion',
                component: PromotionComponent,
            },
            {
                path: 'soldout',
                component: SoldOutComponent,
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class productsRoutingModule { }



