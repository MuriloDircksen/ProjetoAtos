import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IIngredientes } from 'src/app/models/ingredientes';
import { IReceitaIngrediente } from 'src/app/models/receita-ingrediente';
import { IReceitas } from 'src/app/models/receitas';
import { IngredienteServiceService } from 'src/app/service/ingrediente/ingrediente-service.service';
import { ReceitasServiceService } from 'src/app/service/receita/receita-service.service';

@Component({
  selector: 'app-modifica-receita',
  templateUrl: './modifica-receita.component.html',
  styleUrls: ['./modifica-receita.component.scss']
})
export class ModificaReceitaComponent implements OnInit{

  titulo!: string;
  receitaId!: any;
  formReceita!: FormGroup;
  receita!: IReceitas;
  listaReceitas!: IReceitas[];
  listaIngredientes! : IIngredientes[];
  listaReceitaIngrediente : IReceitaIngrediente[] = [];
  quantidadeIngredientes : number = 0;
  orcamento : number = 0;
  quantidadeReceitas : number = 0;



  constructor(private activatedRoute: ActivatedRoute, private receitaService: ReceitasServiceService,
    private formBuilder: FormBuilder, private router: Router,
    private ingredienteService : IngredienteServiceService){
      this.buscaIngredientes();
      this.retornaQuantidadeIngredientes();
    }

  ngOnInit(): void {
    this.receitaId = this.activatedRoute.snapshot.paramMap.get('id')
    this.buscaReceita();
    this.formReceita;

  }

  buscaIngredientes(){
    this.ingredienteService.getIngredientes().subscribe((data)=>{
      this.listaIngredientes = data;
    })
  }
  buscaReceita(){
    if(this.receitaId == "criar"){
      this.verificaTemId();
      this.receitaService.getReceitas().subscribe((data)=>{
        this.quantidadeReceitas= data[data.length-1].id;
      })
      return;
   }
   this.receitaService.getReceita(parseFloat(this.receitaId)).subscribe((data)=>{
    this.receita = data;
    this.orcamento = data.orcamento;
    this.verificaTemId();
   })

  }

  verificaTemId(){

    if(this.receitaId == "criar"){
      this.titulo = "Criar Receita"
      this.criaFormCadastro();
      return;
    }
    this.titulo = "Editar Receita"
    this.criaFormEdicao();
   }

    retornaQuantidadeIngredientes(){
      if(this.receitaId == "criar"){
        return;
      }
      this.receitaService.getReceitaIngredientes().subscribe((data)=>{
        this.listaReceitaIngrediente = data;
        data.forEach((elemento)=>{
        if(elemento.receitaId == this.receitaId){
          this.quantidadeIngredientes +=1;
        }
       })
     })
    }

    criaFormCadastro(){
      this.formReceita = this.formBuilder.group({
       nome: new FormControl('', Validators.required),
       responsavelReceita: new FormControl('', [Validators.required]),
       estilo: new FormControl('', Validators.required),
       volumeReceita: new FormControl('', Validators.required),
       quantidadeIngredientes: [0 , Validators.required],
       ingredientes: this.formBuilder.array([])
      })
    }

  criaFormEdicao(){
    this.formReceita = this.formBuilder.group({
      nome: new FormControl(this.receita.nomeReceita, Validators.required),
      responsavelReceita: new FormControl(this.receita.responsavelReceita, Validators.required),
      estilo: new FormControl(this.receita.estilo, Validators.required),
      volumeReceita: new FormControl(this.receita.volumeReceita, Validators.required),
      quantidadeIngredientes: new FormControl(this.quantidadeIngredientes, Validators.required),
      ingredientes: this.formBuilder.array([])
    });
    this.carregarIngredienteFormulario();
  }

  get quantidades(){
    return this.formReceita.get('quantidadeIngredientes')?.value;
  }
  get volumeReceita(){
    return this.formReceita.get('volumeReceita')?.value;
  }
  get numeroIngredientes():FormArray {
    return this.formReceita.get('ingredientes') as FormArray;
  }
  get nome(){
    return this.formReceita.get('nome')?.value;
  }
  get responsavelReceita(){
    return this.formReceita.get('responsavelReceita')?.value;
  }
  get estilo(){
    return this.formReceita.get('estilo')?.value;
  }
  get ultimaAtualizacao(){
    return this.formReceita.get('ultimaAtualizacao')?.value;
  }

  onChangecontagemIngrediente() {
    const contagemIngrediente = this.quantidades;
    const contagemAtualIngrediente = this.numeroIngredientes;

    if (contagemIngrediente < contagemAtualIngrediente.length) {
      for (let i = contagemAtualIngrediente.length - 1; i >= contagemIngrediente; i--) {
        contagemAtualIngrediente.removeAt(i);
      }
    } else {
      for (let i = contagemAtualIngrediente.length; i < contagemIngrediente; i++) {
        contagemAtualIngrediente.push(
          this.formBuilder.group({
            selectedIngredient: ['', Validators.required],
            quantidade: ['', Validators.required]
          })
        );
      }
    }
    this.calculaOrcamento();
  }

    calculaOrcamento(){

        this.orcamento = 0;
        this.numeroIngredientes.controls.forEach((control)=>{
          const obj = control.value;
          this.listaIngredientes.map((elemento)=>{
            if(elemento.id == obj.selectedIngredient){
              this.orcamento += obj.quantidade*elemento.valor_unidade;
            }
          })
        })
        return;

    }

    criaIngrediente(): FormGroup {
      return this.formBuilder.group({
        selectedIngredient: '',
        quantidade: ''
      });
    }
    carregarIngredienteFormulario(){

      this.listaReceitaIngrediente.forEach((elemento)=>{
        this.listaIngredientes.forEach((ingrediente)=>{
          if(elemento.ingredienteId == ingrediente.id){
            this.adicionaIngrediente();
            const control = this.numeroIngredientes;
            const index = control.length-1;

            control.at(index).patchValue({
              selectedIngredient: ingrediente.id,
              quantidade: elemento.quantidadeIngrediente
            });
          }
        })
      })
    }

    adicionaIngrediente() {
      const control = this.numeroIngredientes;
      control.push(this.criaIngrediente());
    }

  async modificaReceita(){
    if(this.receitaId === "criar"){
      const Receita: any= {
        nomeReceita: this.nome,
        responsavelReceita: this.responsavelReceita,
        estilo: this.estilo,
        ultima_atualizacao: new Date(),
        orcamento: this.orcamento,
        volumeReceita: this.volumeReceita
      }

      await this.receitaService.salvarReceita(Receita).toPromise();

      this.modificaIngredienteReceita();
      this.retornaPaginaReceita();
      return;
    }

     const Receita: any= {
        id: this.receita.id,
        nomeReceita: this.nome,
        responsavelReceita: this.responsavelReceita,
        estilo: this.estilo,
        ultima_atualizacao: new Date(),
        orcamento: this.orcamento,
        volumeReceita: this.volumeReceita
     }

    this.receitaService.atualizarReceita(Receita).subscribe();
    this.alteraIngredientesReceita()
    this.retornaPaginaReceita();

  }

     modificaIngredienteReceita(){
      this.numeroIngredientes.controls.forEach((control)=>{
        const ingredienteReceita : any = {
          receitaId: this.quantidadeReceitas+1,
          ingredienteId: control.value.selectedIngredient,
          quantidadeIngrediente: control.value.quantidade
        }
       this.receitaService.salvarReceitaIngredientes(ingredienteReceita).toPromise();
      })
    }

    alteraIngredientesReceita(){
      let listaControleIngredientes: any = [];
       this.numeroIngredientes.controls.forEach((control)=>{
        const ingrediente: any = {
            receitaId: parseInt(this.receitaId),
           ingredienteId: parseInt(control.value.selectedIngredient),
           quantidadeIngrediente: parseInt(control.value.quantidade)
         }
        listaControleIngredientes.push(ingrediente);
        })

        const ingredienteAdicionados = listaControleIngredientes.filter((ingredienteTeste : any) => !this.listaReceitaIngrediente.some(prevIng => prevIng.ingredienteId === ingredienteTeste.ingredienteId));
        console.log("adicionar");

        console.log(ingredienteAdicionados);
        const removerIngredientes = this.listaReceitaIngrediente.filter(prevIng => !listaControleIngredientes.some((ingredienteTeste: any) => ingredienteTeste.ingredienteId === prevIng.ingredienteId));
        console.log("remover");
        console.log(removerIngredientes);
        const alterarIngredientes = listaControleIngredientes.filter((ingredient:any) => {
          const ingredientesAnterior = this.listaReceitaIngrediente.find(prevIng => prevIng.ingredienteId === ingredient.ingredienteId);
          return ingredientesAnterior && (ingredientesAnterior.ingredienteId !== ingredient.ingredienteId || ingredientesAnterior.quantidadeIngrediente !== ingredient.quantidadeIngrediente);
        });
        console.log("mudar");
        console.log(alterarIngredientes);

        this.adicionaNovoIngredienteReceita(ingredienteAdicionados);
        this.alteraNovoIngredienteReceita(alterarIngredientes);
        this.removeNovoIngredienteReceita(removerIngredientes);

    }
    async adicionaNovoIngredienteReceita(novaLista: any){
      novaLista.forEach((elemento: IReceitaIngrediente) => {
        console.log(elemento);
        this.receitaService.salvarReceitaIngredientes(elemento).toPromise();
      });
    }
   async alteraNovoIngredienteReceita(novaLista: any){
      novaLista.forEach((elemento: any) => {
        this.listaReceitaIngrediente.forEach((data)=>{
          if(elemento.ingredienteId == data.ingredienteId){
            const ingredienteReceita : any = {
              id: data.id,
              receitaId:elemento.receitaId,
              ingredienteId: elemento.ingredienteId,
              quantidadeIngrediente: elemento.quantidadeIngrediente
            }
            console.log(ingredienteReceita);

            this.receitaService.atualizarReceitaIngrediente(ingredienteReceita).toPromise();
          }
        })
      });
    }

  async removeNovoIngredienteReceita(novaLista: any){
      novaLista.forEach((elemento : any)=>{
        console.log(elemento);
        this.receitaService.excluirReceitaIngrediente(elemento.id).toPromise();
      })
    }



   excluiReceita(){

    this.receitaService.excluirReceita(this.receitaId).subscribe(); //está fazendo cascade all verificar depois regras com back end
   // this.exlcuiIngredientes();
    this.router.navigate(['/Receita']);

    // this.retornaPaginaReceita();
   }
  //  async exlcuiIngredientes(){ //está fazendo cascade all
  //   this.listaReceitaIngrediente.forEach((data)=>{
  //     this.receitaService.excluirReceitaIngrediente(data.id).toPromise();
  //   })
  //  }

  retornaPaginaReceita(){

      this.router.navigate(['/receitas']);

  }

}
