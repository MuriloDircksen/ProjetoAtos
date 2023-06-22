import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UsuarioService } from 'src/app/service/usuario.service';

@Component({
  selector: 'app-cadastro-usuario',
  templateUrl: './cadastro-usuario.component.html',
  styleUrls: ['./cadastro-usuario.component.scss']
})
export class CadastroUsuarioComponent implements OnInit {

  formUsuario!: FormGroup;

  constructor(private usuarioService: UsuarioService, private router:Router, private formBuilder: FormBuilder){}

  ngOnInit(): void {
    this.criaFormCadastro();
  }

  criaFormCadastro(){
    this.formUsuario = this.formBuilder.group({
      email: new FormControl('', [Validators.required, Validators.email]),
      empresa: new FormControl('', [Validators.required]),
      cnpj: new FormControl('', [Validators.required, Validators.minLength(14), Validators.maxLength(14)]),
      nome: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(200)]),
      senha: new FormControl('', [Validators.required, Validators.minLength(8)]),
      confirmaSenha: new FormControl('', [Validators.required, Validators.minLength(8)]),
    });
  }

  get email(){
    return this.formUsuario.get('email')?.value;
  }
  get empresa(){
    return this.formUsuario.get('empresa')?.value;
  }
  get cnpj(){
    return this.formUsuario.get('cnpj')?.value;
  }
  get nome(){
    return this.formUsuario.get('nome')?.value;
  }
  get senha(){
    return this.formUsuario.get('senha')?.value;
  }
  get confirmaSenha(){
    return this.formUsuario.get('confirmaSenha')?.value;
  }

  validaSenha(){
    if(this.confirmaSenha.value == this.senha.value){
      return true;
    }
    alert("Senha inválida!")
    return false
  }

  onSubmit(){
    if(this.validaSenha()){

      const usuario = {
        nome: this.nome,
        senha: this.senha,
        nomeEmpresa: this.empresa,
        cnpj: this.cnpj,
        email: this.email
      }
      this.usuarioService.salvarUsuario(usuario).subscribe();

      this.router.navigate(['/login'])
    }
  }

}
