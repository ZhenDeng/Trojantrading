import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators/catchError';
import { ApiResponse } from '../models/ApiResponse';
import { User } from '../models/user';

@Injectable()
export class AdminService {

  base_url: string = "api/Admin";

  constructor(private http: HttpClient) { }

  GetUserByAccount(userId: Number): Observable<User> {
    return this.http.get(this.base_url + "/GetUserByAccount?userId=" + userId)
      .pipe(catchError(this.handleError));
  }

  UpdateUser(user: User): Observable<ApiResponse> {
    return this.http.post(this.base_url + "/UpdateUser", user)
      .pipe(catchError(this.handleError));
  }

  GetUserWithAddress(userId: Number): Observable<User> {
    return this.http.get(this.base_url + "/GetUserWithAddress?userId=" + userId)
      .pipe(catchError(this.handleError));
  }

  GetUserWithRole(userId: Number): Observable<User> {
    return this.http.get(this.base_url + "/GetUserWithRole?userId=" + userId)
      .pipe(catchError(this.handleError));
  }

  GetUsersWithRole(): Observable<User[]> {
    return this.http.get(this.base_url + "/GetUsersWithRole")
      .pipe(catchError(this.handleError));
  }

  ValidatePassword(userId: Number, password: string): Observable<ApiResponse> {
    return this.http.get(this.base_url + "/ValidatePassword?userId=" + userId + "&password=" + password)
      .pipe(catchError(this.handleError));
  }

  UpdatePassword(userId: Number, password: string): Observable<ApiResponse> {
    return this.http.get(this.base_url + "/UpdatePassword?userId=" + userId + "&password=" + password)
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
