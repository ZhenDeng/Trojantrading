import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import { ShareService } from '../services/share.service';
import { LoadScreenService } from '../services/load-screen.service';
import { finalize } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  activeRequests: number = 0;
 
  /**
   * URLs for which the loading screen should not be enabled
   */
  skippUrls = [
    
  ];

  constructor(private router: Router, private shareService: ShareService, private loadingScreenService: LoadScreenService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    let displayLoadingScreen = true;
 
    for (const skippUrl of this.skippUrls) {
      if (new RegExp(skippUrl).test(request.url)) {
        displayLoadingScreen = false;
        break;
      }
    }
 
 
    if (displayLoadingScreen) {
      if (this.activeRequests === 0) {
        this.loadingScreenService.startLoading();
      }
      this.activeRequests++;
      if(request.headers.get('No-Auth') == "True"){
        return next.handle(request.clone()).pipe(
          finalize(() => {
            this.activeRequests--;
            if (this.activeRequests === 0) {
              this.loadingScreenService.stopLoading();
            }
          })
        )
      }
      if(this.shareService.readCookie("userToken")) {
        return next.handle(
            request.clone(
                { headers: request.headers.set("Authorization", "Bearer " + this.shareService.readCookie("userToken"))}
            )
        ).pipe(
          finalize(() => {
            this.activeRequests--;
            if (this.activeRequests === 0) {
              this.loadingScreenService.stopLoading();
            }
          })
        )
      }
      else{
          this.router.navigateByUrl('/login');
      }
    } else {
      return next.handle(request).pipe(
        finalize(() => {
          this.activeRequests--;
          if (this.activeRequests === 0) {
            this.loadingScreenService.stopLoading();
          }
        })
      );
    }

  }
}