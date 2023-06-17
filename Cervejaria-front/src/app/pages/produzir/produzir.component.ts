import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { IReceitas } from 'src/app/models/receitas';
import { ProducaoServiceService } from 'src/app/service/producao/producao-service.service';
import { ReceitasServiceService } from 'src/app/service/receita/receita-service.service';

@Component({
  selector: 'app-produzir',
  templateUrl: './produzir.component.html',
  styleUrls: ['./produzir.component.scss']
})
export class ProduzirComponent implements OnInit,OnDestroy{

  listaReceitas!: IReceitas[];
  receitaProducao: any = {};
  formProducao!: FormGroup;
  subReceitas!:Subscription;


  constructor(private formBuilder: FormBuilder, private router: Router,
    private receitaService : ReceitasServiceService, private producaoService : ProducaoServiceService){}

  ngOnInit(): void {
    this.buscaReceitas();
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
  const receita:any ={
      quantidade: this.volumeApronte,
      receitaId: this.receita,
      responsavel: this.responsavel,
      dataProducao: new Date()
  }

  this.producaoService.salvarProducao(receita).toPromise();
  this.retornaDashboard()
}

retornaDashboard(){
  this.router.navigate(['/home']);
}
 ngOnDestroy(): void {
  this.subReceitas.unsubscribe();
 }
}
