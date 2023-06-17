import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { IEstoque } from 'src/app/models/estoque';
import { IIngredientes } from 'src/app/models/ingredientes';
import { EstoqueServiceService } from 'src/app/service/estoque/estoque-service.service';
import { IngredienteServiceService } from 'src/app/service/ingrediente/ingrediente-service.service';

@Component({
  selector: 'app-estoque',
  templateUrl: './estoque.component.html',
  styleUrls: ['./estoque.component.scss']
})
export class EstoqueComponent implements OnDestroy, OnInit  {

   listaIngredientes!: IIngredientes[] ;
   listaEstoque!: IEstoque[];
   subIngredientes!: Subscription;
   subEstoque!: Subscription;
   listaCombinada: any[] = [];

   constructor(private ingredienteService: IngredienteServiceService,
    private estoqueService : EstoqueServiceService, private router: Router){  }

   ngOnInit(): void {
      this.buscaIngredientes();
      this.buscaEstoque();
   }

  buscaIngredientes(){
   this.subIngredientes = this.ingredienteService.getIngredientes().subscribe((data)=>{
     this.listaIngredientes = data;
   })
  }

  buscaEstoque(){
    this.subEstoque = this.estoqueService.getEstoques().subscribe((data)=>{
      this.listaEstoque = data;
    })
  }
  combinarListas(){
    this.listaEstoque.forEach(item => {
      const ingrediente = this.listaIngredientes.find(ing => ing.idEstoque === item.id);
      if (ingrediente) {
        const itemCombinado = {
          id: item.id,
          ingrediente: ingrediente,

        };
        this.listaCombinada.push(itemCombinado);
      }
    });
  }

ngOnDestroy(): void {
   this.subIngredientes.unsubscribe();
   this.subEstoque.unsubscribe();
}

}
