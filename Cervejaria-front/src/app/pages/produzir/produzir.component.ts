import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { IIngredientes } from 'src/app/models/ingredientes';
import { IReceitaIngrediente } from 'src/app/models/receita-ingrediente';
import { IReceitas } from 'src/app/models/receitas';
import { IngredienteServiceService } from 'src/app/service/ingrediente/ingrediente-service.service';
import { ProducaoServiceService } from 'src/app/service/producao/producao-service.service';
import { ReceitasServiceService } from 'src/app/service/receita/receita-service.service';

@Component({
  selector: 'app-produzir',
  templateUrl: './produzir.component.html',
  styleUrls: ['./produzir.component.scss']
})
export class ProduzirComponent implements OnInit,OnDestroy{

  listaReceitas!: IReceitas[];
  listaReceitaIngrediente!: IReceitaIngrediente[];
  listaIngredientes!: IIngredientes[];
  receitaProducao: any = {};
  controle:boolean = true;
  formProducao!: FormGroup;
  subReceitas!:Subscription;
  subIngredientes!: Subscription;
  subReceitaIngrediente!: Subscription;


  constructor(private formBuilder: FormBuilder, private router: Router,
    private receitaService : ReceitasServiceService, private producaoService : ProducaoServiceService,
    private ingredienteService:IngredienteServiceService){}

  ngOnInit(): void {
    this.buscaReceitas();
    this.buscarIngredientes();
    this.buscarReceitaIngredientes();
    this.criaFormCadastro();
  }

  buscaReceitas(){
    this.subReceitas = this.receitaService.getReceitas().subscribe((data)=>{
      this.listaReceitas = data;
    })
  }
  criaFormCadastro(){
    this.formProducao = this.formBuilder.group({
      receita: new FormControl('', Validators.required),
      estilo: new FormControl( {value: this.receitaProducao.estilo, disabled: true}),
      volume: new FormControl( {value: this.receitaProducao.volumeReceita, disabled: true}),
      volumeApronte: new FormControl('' , [Validators.required]),
      responsavel: new FormControl('', Validators.required),
      orcamento: new FormControl( {value: this.receitaProducao.orcamento, disabled: true})
    });
 }
 get receita(){
  return this.formProducao.get('receita')?.value;
}
get estilo(){
  return this.formProducao.get('estilo')?.value;
}
get volumeApronte(){
  return this.formProducao.get('volumeApronte')?.value;
}
get responsavel(){
  return this.formProducao.get('responsavel')?.value;
}
get orcamento(){
  return this.formProducao.get('orcamento')?.value;
}
alteraItensCarregados(){
   this.receitaService.getReceita(this.receita).subscribe((data)=>{
    this.receitaProducao = data;
    this.formProducao.patchValue({
      estilo: data.estilo,
      volume: data.volumeReceita,
      orcamento: data.orcamento
    });
  })
}

async produzirReceita(){

  this.retirarMateriaPrimaEstoque();
  if(this.controle){
    const receita:any ={
        quantidade: this.volumeApronte,
        receitaId: this.receita,
        responsavel: this.responsavel,
        dataProducao: new Date()
    }

    this.producaoService.salvarProducao(receita).toPromise();
}
  this.retornaDashboard()
}
retirarMateriaPrimaEstoque(){

  const ingredientesutilizados = this.listaReceitaIngrediente.filter((item)=> item.receitaId == this.receita);

  ingredientesutilizados.forEach((ingrediente)=>{
    const testaIngrediente = this.listaIngredientes.find((item) => item.id==ingrediente.ingredienteId);
    if (testaIngrediente) {
      if(testaIngrediente.quantidade - ingrediente.quantidadeIngrediente < 0){
        window.alert(`Ingrediente ${testaIngrediente.nomeIngrediente} possui quantidade insuficiente no estoque`)
       this.controle = false;
      }
    }
  })
  if(this.controle){
    ingredientesutilizados.forEach((ingrediente)=>{
      const ingredienteAtual = this.listaIngredientes.find((item) => item.id==ingrediente.ingredienteId);
      if (ingredienteAtual) {
          const estoqueAtualizao = {
            id: ingredienteAtual.id,
            nomeIngrediente: ingredienteAtual?.nomeIngrediente,
            idEstoque: ingredienteAtual.idEstoque,
            quantidade: ingredienteAtual.quantidade - ingrediente.quantidadeIngrediente,
            tipo: ingredienteAtual.tipo,
            valor_unidade: ingredienteAtual.valor_unidade,
            valorTotal: ingredienteAtual.valorTotal,
            unidade: ingredienteAtual.unidade,
            fornecedor: ingredienteAtual.fornecedor,
            validade:ingredienteAtual.validade,
            data_entrada: ingredienteAtual.data_entrada
          }
          this.ingredienteService.atualizarIngrediente(estoqueAtualizao).subscribe();
      }
    })
  }

}
buscarReceitaIngredientes(){
  this.subReceitaIngrediente = this.receitaService.getReceitaIngredientes().subscribe((data)=>{
    this.listaReceitaIngrediente = data;
  });
}
buscarIngredientes(){
  this.subIngredientes = this.ingredienteService.getIngredientes().subscribe((data)=>{
    this.listaIngredientes = data;
  });
}

retornaDashboard(){
  this.router.navigate(['/home']);
}
 ngOnDestroy(): void {
  this.subReceitas.unsubscribe();
  this.subIngredientes.unsubscribe();
  this.subReceitaIngrediente.unsubscribe();
 }
}
