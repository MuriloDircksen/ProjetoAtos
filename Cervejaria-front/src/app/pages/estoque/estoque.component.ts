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
   listaUnificadaIngredientes: IIngredientes[] = [];
   listaEstoque!: IEstoque[];
   subIngredientes!: Subscription;
   subEstoque!: Subscription;
   listaCombinada: any[] = [];

   constructor(private ingredienteService: IngredienteServiceService,
    private estoqueService : EstoqueServiceService, private router: Router){
      this.buscaIngredientes();
      this.buscaEstoque();
    }

   ngOnInit(): void {

   }

  buscaIngredientes(){
   this.subIngredientes = this.ingredienteService.getIngredientes().subscribe((data)=>{
     this.listaIngredientes = data;
     this.combinarIngredientes();
   })
  }

  buscaEstoque(){
    this.subEstoque = this.estoqueService.getEstoques().subscribe((data)=>{
      this.listaEstoque = data;
      this.combinarListas();
    })
  }
  combinarIngredientes() {
    this.listaIngredientes.forEach(ingrediente => {
      const ingredienteExistente = this.listaUnificadaIngredientes.find(item => item.nomeIngrediente === ingrediente.nomeIngrediente);

      if (ingredienteExistente) {
        ingredienteExistente.quantidade += ingrediente.quantidade;
        ingredienteExistente.valorTotal += ingrediente.valorTotal;
      } else {
        const novoIngrediente: IIngredientes = { ...ingrediente };
        this.listaUnificadaIngredientes.push(novoIngrediente);
      }
    });

  }


  combinarListas(){
    for (const estoque of this.listaEstoque) {
      const ingredientesAssociados = this.listaIngredientes.filter(item => item.idEstoque === estoque.id);

      if (ingredientesAssociados.length > 0) {
        for (const ingrediente of ingredientesAssociados) {
          const itemComb = this.listaUnificadaIngredientes.find(item => item.id === ingrediente.id);

          if (itemComb) {
            this.listaCombinada.push({
              id: estoque.id,
              nomeEstoque: estoque.nome,
              nomeIngrediente: itemComb.nomeIngrediente,
              quantidade: itemComb.quantidade,
              valorTotal: itemComb.valorTotal
            });

          } else {
            this.listaCombinada.push({
              id: estoque.id,
              nomeEstoque: estoque.nome,
              nomeIngrediente: ingrediente.nomeIngrediente,
              quantidade: ingrediente.quantidade,
              valorTotal: ingrediente.valorTotal
            });
          }
        }
      } else {
        this.listaCombinada.push({
          id: estoque.id,
          nomeEstoque: estoque.nome,
          nomeIngrediente: "Sem ingrediente",
          quantidade: 0,
          valorTotal:0
        });
      }
    }
  }


  criarNovoEstoque(){
    const nomeEstoque = prompt('Digite o nome do novo estoque:');
    console.log(nomeEstoque, nomeEstoque?.length);

    if(nomeEstoque && nomeEstoque.length>=3){
      const estoque: any={
        nome:nomeEstoque
      }
      this.estoqueService.salvarEstoque(estoque).subscribe();
      this.zerarListas();
      this.buscaIngredientes();
      this.buscaEstoque();
      return;
    }
    prompt('Nome inválido, deve ter ao menos 3 caracteres');
  }

  alterarEstoque(id: number) {
    const nomeEstoque = prompt('Digite o novo nome do estoque:');
    if(nomeEstoque&& nomeEstoque.length>=3){
      const estoque:any ={
        id: id,
        nome: nomeEstoque
      }
      this.estoqueService.atualizarEstoque(estoque).subscribe();
      this.zerarListas();
      this.buscaIngredientes();
      this.buscaEstoque();
      return;
    }
    prompt('Nome inválido, deve ter ao menos 3 caracteres');

  }
  zerarListas(){
    this.listaEstoque = [];
    this.listaIngredientes =[];
    this.listaUnificadaIngredientes= [];
    this.listaCombinada= [];
  }
  excluirEstoque(id : number){

    if (this.verificarIngredientesAssociados(id)) {
      alert('Não é possível excluir o estoque, pois possui ingredientes associados.');
      return;
    }
    this.estoqueService.excluirEstoque(id).subscribe();
    this.zerarListas();
    this.buscaIngredientes();
    this.buscaEstoque();

  }
  verificarIngredientesAssociados(id : number):boolean{
    const procuraRelacao = this.listaIngredientes.find(item => item.idEstoque === id);

    if(procuraRelacao) {return true;}

    return false;
  }

ngOnDestroy(): void {
   this.subIngredientes.unsubscribe();
   this.subEstoque.unsubscribe();
}

}
