import { Injectable } from '@angular/core';
import { TokenService } from '../token/token.service';
import { UsuarioService } from '../usuario.service';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private tokenService: TokenService, private usuarioService : UsuarioService, private router:Router) { }

  login(objeto:any) {

   return this.usuarioService.gerarToken(objeto).subscribe((data) => {
        this.tokenService.setToken(data);
        this.router.navigate(['/home'])
    });

  }
}
