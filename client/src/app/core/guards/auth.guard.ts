import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {AccountService} from '../../account/account.service';

// this is just like a service and injected inside root module by providedIn: 'root'
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accountService: AccountService, private router: Router)
  {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(auth =>{
        if(auth){
          return true;
        }
        this.router.navigate(['account/login'], {queryParams: {returnUrl: state.url}});
      })
    );
  }
  
}
