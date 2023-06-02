import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { IReceitas } from 'src/app/models/receitas';
import { ReceitasServiceService } from 'src/app/service/receita/receita-service.service';

@Component({
  selector: 'app-receita',
  templateUrl: './receita.component.html',
  styleUrls: ['./receita.component.scss']
})
export class ReceitaComponent implements OnInit,OnDestroy{

  listaReceitas!: IReceitas[];
  subReceitas!: Subscription;

  constructor(private router: Router, private receitaService : ReceitasServiceService){}

  ngOnInit(): void {
    this.buscaReceitas();
  }

  buscaReceitas(){
    this.subReceitas = this.receitaService.getReceitas().subscribe((data)=>{
      this.listaReceitas = data;
    })
  }
  criarReceita(){
    this.router.navigate(['/receitas/criar'])
  }

  ngOnDestroy(): void {
    this.subReceitas.unsubscribe;
  }

}
