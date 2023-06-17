import { IngredienteServiceService } from 'src/app/service/ingrediente/ingrediente-service.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IIngredientes } from 'src/app/models/ingredientes';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ingrediente',
  templateUrl: './ingrediente.component.html',
  styleUrls: ['./ingrediente.component.scss']
})
export class IngredienteComponent implements OnDestroy, OnInit  {

   listaIngredientes!: IIngredientes[] ;
   subIngredientes!: Subscription;

   constructor(private ingredienteService: IngredienteServiceService, private router: Router){  }

   ngOnInit(): void {
      this.buscaIngredientes();
   }

   buscaIngredientes(){
    this.subIngredientes = this.ingredienteService.getIngredientes().subscribe((data)=>{
      this.listaIngredientes = data;
      this.listaIngredientes.sort((a,b) => (a.nomeIngrediente > b.nomeIngrediente) ? 1 : ((b.nomeIngrediente > a.nomeIngrediente ? -1 : 0)));
    })
   }

   criaIngrediente(){
     this.router.navigate(['/ingredientes/criar']);
   }

 ngOnDestroy(): void {
    this.subIngredientes.unsubscribe();
 }

}
