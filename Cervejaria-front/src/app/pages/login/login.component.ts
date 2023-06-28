import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IUsuario } from 'src/app/models/usuario';
import { AuthService } from 'src/app/service/login/auth.service';
import { TokenService } from 'src/app/service/token/token.service';



@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  usuario!: IUsuario;
  formUsuario!: FormGroup;


  constructor( private tokenService: TokenService, private authService : AuthService, private router:Router){}

  ngOnInit(): void {
    this.criaForm();
    //this.getListaUsuarios();
  }

  criaForm(){
    this.formUsuario = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      senha: new FormControl('', [Validators.required, Validators.minLength(8)])
    });
  }

  get email(){
    return this.formUsuario.get('email')!;
  }

  get senha(){
    return this.formUsuario.get('senha')!;
  }


   onSubmit(){
    const objeto : any ={
      senha: this.senha.value,
      email: this.email.value
    }
    this.authService.login(objeto);
    const token = this.tokenService.getToken();

    if(token){

      this.router.navigate(['/home'])
      return;
    }


  }
}
