import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { ShareService } from '../services/share.service';

@Injectable()
export class AuthRecovery implements CanActivate {
  constructor(private router: Router, private shareService: ShareService){}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
      if(this.shareService.readCookie("recoverToken")) {
        return true;
      }
      else{
        this.router.navigate(['/login']);
        return false;
      }
  }

}
