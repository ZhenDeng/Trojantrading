import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product, Category, PackagingList } from '../models/Product';
import { catchError } from 'rxjs/operators';
import { ApiResponse } from '../models/ApiResponse';

@Injectable()
export class ProductService {

  base_url: string = "api/Product";

  categoryList: Category[] = [
    { type: 'Cigarettes', category: 'Cigarettes' },
    { type: 'RYO', category: 'RYO' },
    { type: 'Pipe', category: 'Pipe' },
    { type: 'Cigars', category: 'Cigars' },
    { type: 'Cuban Cigar', category: 'Cuban-Cigar' },
    { type: 'non-Cuban Cigar', category: 'nonCuban-Cigar' },
    { type: 'Lighters', category: 'Lighters' },
    { type: 'Papers', category: 'Papers' },
    { type: 'Accessories', category: 'Accessories' }
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

  UpdatePackagingList(productId: number, lists: PackagingList[]): Observable<ApiResponse> {
    return this.http.post(this.base_url + "/UpdatePackagingList?productId=" + productId, lists)
      .pipe(catchError(this.handleError));
  }

  DeleteProduct(product: Product): Observable<ApiResponse> {
    return this.http.post(this.base_url + "/DeleteProduct", product)
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
