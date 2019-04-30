import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product, Category } from '../models/Product';
import { catchError } from 'rxjs/operators';
import { ApiResponse } from '../models/ApiResponse';

@Injectable()
export class ProductService {

  base_url: string = "api/Product";

  categoryList: Category[] = [
    { type: 'Hand-Made Cigars', category: 'hand-made' },
    { type: 'Machine-Made Cigars', category: 'machine-made' },
    { type: 'Little Cigars', category: 'little-cigars' },
    { type: 'Cigarettes', category: 'cigarettes' },
    { type: 'Pipe Tobacco', category: 'pipe-tobacco' },
    { type: 'Roll Your Own', category: 'roll-your-won' },
    { type: 'Filters', category: 'filters' },
    { type: 'Papers', category: 'papers' },
    { type: 'Lighters', category: 'lighters' },
    { type: 'Accessories', category: 'accessories' },
  ];

  constructor(
    private http: HttpClient
  ) { }

  getAllProducts(): Observable<Product[]> {
    return this.http.get(this.base_url + "/GetAllProducts")
    .pipe(catchError(this.handleError));
  }

  UpdateProduct(product: Product): Observable<ApiResponse> {
    return this.http.post(this.base_url + "/UpdateProduct", product)
    .pipe(catchError(this.handleError));
  }

  AddProduct(product: Product): Observable<ApiResponse> {
    return this.http.post(this.base_url + "/AddProduct", product)
    .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    console.error('server error:', error);

    if (error.error instanceof ErrorEvent) {
      let errMessage = '';
      try {
        errMessage = error.error.message;
      } catch (err) {
        errMessage = error.statusText;
      }
      return Observable.throw(errMessage);
    }
    return Observable.throw(error.error || 'ASP.NET Core server error');
  }

}
