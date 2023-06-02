import { ReceitasServiceService } from './../../service/receita/receita-service.service';
import { ProducaoServiceService } from './../../service/producao/producao-service.service';
import { Component, OnInit } from '@angular/core';
import { IProducao } from 'src/app/models/producao';
import { IReceitas } from 'src/app/models/receitas';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  listaProducao!: IProducao[];
  volumeProduzido: number = 0;
  totalCusto: number = 0;
  listaReceita!: IReceitas[];
  cervejasProduzidas: number = 0;
  listaProduções: any[] = [];
  ultimasProducoes!: any[];

  constructor(private producaoServiceService: ProducaoServiceService,
    private receitasServiceService: ReceitasServiceService){}

  ngOnInit(): void {
    this.buscaReceitas();

  }

  buscaReceitas(){
    this.receitasServiceService.getReceitas().subscribe((data)=>{
      this.listaReceita = data

      this.buscaProducoes();
    })

  }
  buscaProducoes(){
    this.producaoServiceService.getProducaos().subscribe((data)=>{
      this.listaProducao = data;
      this.cervejasProduzidas = data.length;
      data.forEach((producao)=>{
        this.volumeProduzido += producao.quantidade;
      })
      this.percorrerReceitas();
    })


  }
  percorrerReceitas(){
    this.listaReceita.forEach((element)=> {
      this.criaListaProducoes(element.id, element);
    })

  }
  criaListaProducoes(id:number, receita : IReceitas){

    this.listaProducao.forEach(element => {


      if(element.receitaId == id){
        this.listaProduções.push({
          nomeReceita: receita.nomeReceita,
          estiloReceita: receita.estilo,
          responsavelProducao: element.responsavel,
          volumeProducao: element.quantidade,
          dataProducao: new Date(element.dataProducao)
        })
        this.totalCusto += receita.orcamento;
      }
    });
    this.selecionaUltimasProducoes();
  }

  selecionaUltimasProducoes(){
    const novaLista = this.listaProduções.sort((a,b)=> (a.dataProducao < b.dataProducao ? 1 : (b.dataProducao < a.dataProducao) ? -1 : 0));
    this.ultimasProducoes = novaLista.slice(0,5);
  }


}


