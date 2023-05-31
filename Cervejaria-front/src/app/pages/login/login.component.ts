import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IUsuario } from 'src/app/models/usuario';
import { UsuarioService } from 'src/app/service/usuario.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  usuario!: IUsuario;
  formUsuario!: FormGroup;
  listaUsuarios:IUsuario[]=[];

  constructor(private serviceUsuario: UsuarioService, private router:Router){}

  ngOnInit(): void {
    this.criaForm();
    this.getListaUsuarios();
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

  getListaUsuarios():void{
    this.serviceUsuario.getUsuarios().subscribe((usuarios) => {
      this.listaUsuarios = usuarios;

     })
   }

   validaUsuario(){
    return this.listaUsuarios.find((usuario)=>{

      if(usuario.email === this.email.value){
         if(usuario.senha == this.senha.value){

           return true;
         }

       }
       alert('Email ou senha inválidos!')
       return false;

     })

   }
   onSubmit(){

    if(this.validaUsuario()){

      localStorage.setItem('segurança', 'true');
      this.router.navigate(['/home'])
    }

  }


}
