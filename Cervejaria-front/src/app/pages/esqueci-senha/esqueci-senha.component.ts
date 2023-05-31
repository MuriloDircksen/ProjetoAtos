import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-esqueci-senha',
  templateUrl: './esqueci-senha.component.html',
  styleUrls: ['./esqueci-senha.component.scss']
})
export class EsqueciSenhaComponent implements OnInit {

  formUsuario!: FormGroup;
  textoBotao: string = "Enviar Recuperação";



  constructor(private router:Router){}

  ngOnInit(): void {
    this.criaFormCadastro();
  }

  criaFormCadastro(){
    this.formUsuario = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email])
    });
  }

  get email(){
    return this.formUsuario.get('email')!;
  }

  onSubmit(){
    if(this.email.value != ''){
      this.textoBotao = "Ir para login";

    }
  }
  retornaLogin(){
      this.router.navigate(['/login']);
  }
}
