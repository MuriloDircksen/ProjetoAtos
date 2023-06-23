import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { IEstoque } from 'src/app/models/estoque';
import { IIngredientes } from 'src/app/models/ingredientes';
import { IReceitaIngrediente } from 'src/app/models/receita-ingrediente';
import { EstoqueServiceService } from 'src/app/service/estoque/estoque-service.service';
import { IngredienteServiceService } from 'src/app/service/ingrediente/ingrediente-service.service';
import { ReceitasServiceService } from 'src/app/service/receita/receita-service.service';

@Component({
  selector: 'app-modifica-ingrediente',
  templateUrl: './modifica-ingrediente.component.html',
  styleUrls: ['./modifica-ingrediente.component.scss']
})
export class ModificaIngredienteComponent implements OnInit, OnDestroy {
   titulo!: string;
   ingredienteId!: any;
   formIngrediente!: FormGroup
   ingrediente!: any;
   listaEstoques!: IEstoque[];
   estoque!: IEstoque;
   subEstoque!: Subscription;
   subIngrediente!: Subscription;
   subEstoques!: Subscription;
   subReceitaIngredientes!: Subscription;

  constructor(private activatedRoute: ActivatedRoute, private ingredienteService: IngredienteServiceService,
     private formBuilder: FormBuilder, private router: Router, private estoqueService : EstoqueServiceService,
     private receitaIngredienteService : ReceitasServiceService){}

   ngOnInit(): void {
     this.ingredienteId = this.activatedRoute.snapshot.paramMap.get('id')
     this.formIngrediente;
    this.buscaIngrediente();
    this.buscaEstoques();
   }

   verificaTemId(){

     if(this.ingredienteId == "criar"){
       this.titulo = "Criar Ingrediente"
       this.criaFormCadastro();
       return;
    }
    this.titulo = "Editar Ingrediente"
    this.buscaEstoqueIngrediente();
   }

   buscaEstoqueIngrediente(){
    this.subEstoque = this.estoqueService.getEstoque(this.ingrediente.idEstoque).subscribe((data)=>{
      this.estoque = data;
      this.criaFormEdicao();
    })
   }

   buscaEstoques(){
    this.subEstoques = this.estoqueService.getEstoques().subscribe((data)=>{
      this.listaEstoques = data;
    });
   }

   criaFormCadastro(){
     this.formIngrediente = this.formBuilder.group({
       nomeIngrediente: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(200)]),
       tipoIngrediente: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
       valorUnidade: new FormControl('', Validators.required),
       unidade: new FormControl('', [Validators.required]),
       fornecedor: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(80)]),
       validade: new FormControl('', [Validators.required]),
       idEstoque: new FormControl('', Validators.required),
       quantidade: new FormControl('', Validators.required)
     });
  }

   criaFormEdicao(){
     this.formIngrediente = this.formBuilder.group({
       nomeIngrediente: new FormControl(this.ingrediente.nomeIngrediente , [Validators.required, Validators.minLength(5), Validators.maxLength(200)]),
       tipoIngrediente: new FormControl(this.ingrediente.tipo , [Validators.required, Validators.minLength(3), Validators.maxLength(50)]),
       valorUnidade: new FormControl(this.ingrediente.valorUnidade , Validators.required),
       unidade: new FormControl(this.ingrediente.unidade , [Validators.required]),
       fornecedor: new FormControl(this.ingrediente.fornecedor , [Validators.required, Validators.minLength(3), Validators.maxLength(80)]),
       validade: new FormControl(new Date(this.ingrediente.validade).toISOString().substring(0, 10) , Validators.required),
       idEstoque: new FormControl(this.estoque.id, Validators.required),
       quantidade: new FormControl(this.ingrediente.quantidade, Validators.required)
     });
   }

  get nomeIngrediente(){
    return this.formIngrediente.get('nomeIngrediente')?.value;
  }
  get tipoIngrediente(){
    return this.formIngrediente.get('tipoIngrediente')?.value;
  }
  get idEstoque(){
    return this.formIngrediente.get('idEstoque')?.value;
  }
  get quantidade(){
    return this.formIngrediente.get('quantidade')?.value;
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
    this.subIngrediente = this.ingredienteService.getIngrediente(parseFloat(this.ingredienteId)).subscribe((data)=>{
      this.ingrediente = data;
      this.verificaTemId();
    })
  }

async alterarIngredientes(){
  if(this.ingredienteId === "criar"){
    const novoIngrediente: any= {
      nomeIngrediente: this.nomeIngrediente,
      idEstoque: this.idEstoque,
      quantidade: this.quantidade,
      tipo: this.tipoIngrediente,
      valorUnidade: this.valorUnidade,
      valorTotal: this.quantidade* this.valorUnidade,
      unidade: this.unidade,
      fornecedor: this.fornecedor,
      validade: this.validade,
      dataEntrada: new Date()
    }
    await this.ingredienteService.salvarIngrediente(novoIngrediente).toPromise();
    this.retornaPaginaIngrediente();
    return;
  }
  const novoIngrediente: any= {
    id: this.ingredienteId,
    nomeIngrediente: this.nomeIngrediente,
    idEstoque: this.idEstoque,
    quantidade: this.quantidade,
    tipo: this.tipoIngrediente,
    valor_unidade: this.valorUnidade,
    valorTotal: this.quantidade* this.valorUnidade,
    unidade: this.unidade,
    fornecedor: this.fornecedor,
    validade: this.validade,
    data_entrada: this.ingrediente.data_entrada
  }

  await this.ingredienteService.atualizarIngrediente(novoIngrediente).toPromise();
  this.retornaPaginaIngrediente();
}

buscaRelacaoReceitaIngrediente(){
  this.subReceitaIngredientes = this.receitaIngredienteService.getReceitaIngredientes().subscribe((data)=>{

    const listaReceitaIngrediente = data.filter((elemento) => elemento.idIngrediente == this.ingredienteId);
    this.excluiIngrediente(listaReceitaIngrediente);
  })
}

async excluiIngrediente(listaVerificacao : IReceitaIngrediente[]){

  if(listaVerificacao.length>0){
    window.alert("Não é permitido excluir ingrediente ligado á uma receita!")
    this.retornaPaginaIngrediente();
    return;
  }
  await this.ingredienteService.excluirIngrediente(this.ingredienteId);


  this.router.navigate(['/ingredientes']);
}

retornaPaginaIngrediente(){
  this.router.navigate(['/ingredientes']);
}

ngOnDestroy(): void {
if(this.ingredienteId != "criar"){
  this.subIngrediente.unsubscribe();
  this.subEstoque.unsubscribe();
}

  this.subEstoques.unsubscribe();
 }

}



