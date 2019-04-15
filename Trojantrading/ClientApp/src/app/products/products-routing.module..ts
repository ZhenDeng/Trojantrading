import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
// import { ProductsComponent } from './products.component';
import { HomeComponent } from '../home/home.component';
import { ProductsComponent } from './products.component';

const routes: Routes = [
    {
        path: 'product',
        component: HomeComponent,
    },
    {
        path: 'productsview/:type',
        component: ProductsComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class productsRoutingModule { }



