import { ActivatedRouteSnapshot, CanActivate, CanActivateFn, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { TokenService } from '../service/token/token.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class authUsuarioGuard implements CanActivate {

  constructor(private router: Router, private tokenService: TokenService){}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

      //const retornaValidacaoLocalStorage = JSON.parse(localStorage.getItem('segurança') as string);
      const retornaValidacao = this.tokenService.getToken();
      if(!retornaValidacao){
        this.router.navigate(['/login'])
        return false
      }
      return true;
  }

}
