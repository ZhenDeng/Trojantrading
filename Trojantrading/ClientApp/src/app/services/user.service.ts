import { Injectable } from '@angular/core';
import { User } from '../Models/User';
import { HttpErrorResponse, HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs/Observable';
import { UserResponse } from '../models/ApiResponse';

@Injectable()
export class UserService {

  selectedUser: User;
  base_url: string = "api/User";

  constructor(private http: HttpClient) { }

  userAuthentication(user: any): Observable<UserResponse> {
    console.info(user);
    return this.http.post(this.base_url + "/authenticate", user)
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