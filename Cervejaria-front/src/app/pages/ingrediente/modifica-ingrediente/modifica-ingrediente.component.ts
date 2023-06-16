import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IIngredientes } from 'src/app/models/ingredientes';
import { IngredienteServiceService } from 'src/app/service/ingrediente/ingrediente-service.service';

@Component({
  selector: 'app-modifica-ingrediente',
  templateUrl: './modifica-ingrediente.component.html',
  styleUrls: ['./modifica-ingrediente.component.scss']
})
export class ModificaIngredienteComponent {
   titulo!: string;
   ingredienteId!: any;
   formIngrediente!: FormGroup
   ingrediente!: any;
   listaIngredientes!: IIngredientes[];


  constructor(private activatedRoute: ActivatedRoute, private ingredienteService: IngredienteServiceService,
     private formBuilder: FormBuilder, private router: Router){}

   ngOnInit(): void {
     this.ingredienteId = this.activatedRoute.snapshot.paramMap.get('id')
     this.formIngrediente;
    this.buscaIngrediente();
   }

   verificaTemId(){

     if(this.ingredienteId == "criar"){
       this.titulo = "Criar Ingrediente"
       this.criaFormCadastro();
       return;
    }
    this.titulo = "Editar Ingrediente"
    this.criaFormEdicao();
   }

   criaFormCadastro(){
     this.formIngrediente = this.formBuilder.group({
       nomeIngrediente: new FormControl('', Validators.required),
       tipoIngrediente: new FormControl('', [Validators.required]),
       valorUnidade: new FormControl('', Validators.required),
       unidade: new FormControl('', [Validators.required]),
       fornecedor: new FormControl('', Validators.required),
       validade: new FormControl('', Validators.required)
     });
  }

   criaFormEdicao(){
     this.formIngrediente = this.formBuilder.group({
       nomeIngrediente: new FormControl(this.ingrediente.nomeIngrediente , Validators.required),
       tipoIngrediente: new FormControl(this.ingrediente.tipo , [Validators.required]),
       valorUnidade: new FormControl(this.ingrediente.valor_unidade , Validators.required),
       unidade: new FormControl(this.ingrediente.unidade , [Validators.required]),
       fornecedor: new FormControl(this.ingrediente.fornecedor , Validators.required),
       validade: new FormControl(this.ingrediente.validade , Validators.required)
     });
   }



  get nomeIngrediente(){
    return this.formIngrediente.get('nomeIngrediente')?.value;
  }
  get tipoIngrediente(){
    return this.formIngrediente.get('tipoIngrediente')?.value;
  }
  get valorUnidade(){
    return this.formIngrediente.get('valorUnidade')?.value;
  }
  get unidade(){
    return this.formIngrediente.get('unidade')?.value;
  }
  get fornecedor(){
    return this.formIngrediente.get('fornecedor')?.value;
  }
  get validade(){
    return this.formIngrediente.get('validade')?.value;
  }

  buscaIngrediente(){
    if(this.ingredienteId == "criar"){
      this.verificaTemId();
      return;
    }
    this.ingredienteService.getIngrediente(parseFloat(this.ingredienteId)).subscribe((data)=>{
      this.ingrediente = data;
      this.verificaTemId();
    })
  }

async alterarIngredientes(){
  if(this.ingredienteId === "criar"){
    const novoIngrediente: any= {
      nomeIngrediente: this.nomeIngrediente,
      tipo: this.tipoIngrediente,
      valor_unidade: this.valorUnidade,
      unidade: this.unidade,
      fornecedor: this.fornecedor,
      validade: this.validade,
      data_entrada: new Date()
    }
    await this.ingredienteService.salvarIngrediente(novoIngrediente).toPromise();
    this.retornaPaginaColecao();
    return;
  }
  const novoIngrediente: any= {
    id: this.ingredienteId,
    nomeIngrediente: this.nomeIngrediente,
    tipo: this.tipoIngrediente,
    valor_unidade: this.valorUnidade,
    unidade: this.unidade,
    fornecedor: this.fornecedor,
    validade: this.validade,
    data_entrada: this.ingrediente.data_entrada
  }

  this.ingredienteService.atualizarIngrediente(novoIngrediente).toPromise();
  this.retornaPaginaColecao();
}

excluiIngrediente(){
  this;this.ingredienteService.excluirIngrediente(this.ingredienteId).subscribe();
  this.router.navigate(['/ingredientes']);
}

   retornaPaginaColecao(){
       this.router.navigate(['/ingredientes']);
   }

}



