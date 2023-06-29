import { Subscription } from 'rxjs';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IIngredientes } from 'src/app/models/ingredientes';
import { IReceitaIngrediente } from 'src/app/models/receita-ingrediente';
import { IReceitas } from 'src/app/models/receitas';
import { IngredienteServiceService } from 'src/app/service/ingrediente/ingrediente-service.service';
import { ReceitasServiceService } from 'src/app/service/receita/receita-service.service';
import { ProducaoServiceService } from 'src/app/service/producao/producao-service.service';
import { IProducao } from 'src/app/models/producao';


@Component({
  selector: 'app-modifica-receita',
  templateUrl: './modifica-receita.component.html',
  styleUrls: ['./modifica-receita.component.scss']
})
export class ModificaReceitaComponent implements OnInit, OnDestroy{

  titulo!: string;
  receitaId: any = 0;
  formReceita!: FormGroup;
  receita!: IReceitas;
  listaReceitas!: IReceitas[];
  listaIngredientes! : IIngredientes[];
  listaReceitaIngrediente : IReceitaIngrediente[] = [];
  listaProducao!: IProducao[];
  quantidadeIngredientes : number = 0;
  orcamento : number = 0;
  quantidadeReceitas : number = 0;
  private subReceita!:Subscription;
  private subIngredientes!:Subscription;
  private subIngredienteReceita!: Subscription;
  private subProducao!: Subscription;




  constructor(private activatedRoute: ActivatedRoute, private receitaService: ReceitasServiceService,
    private formBuilder: FormBuilder, private router: Router,
    private ingredienteService : IngredienteServiceService, private producaoService : ProducaoServiceService){
      this.buscaIngredientes();
      this.retornaQuantidadeIngredientes();

    }

  ngOnInit(): void {
    this.receitaId = this.activatedRoute.snapshot.paramMap.get('id')
    this.buscaReceita();
    this.formReceita;

  }

  buscaIngredientes(){
    this.subIngredientes = this.ingredienteService.getIngredientes().subscribe((data)=>{
      this.listaIngredientes = data;
    })
  }
  buscaReceita(){
    if(this.receitaId == "criar"){
      this.verificaTemId();
      this.subReceita = this.receitaService.getReceitas().subscribe((data)=>{
        this.quantidadeReceitas= data[data.length-1].id;
      })
      return;
   }
   this.subReceita = this.receitaService.getReceita(parseFloat(this.receitaId)).subscribe((data)=>{
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
      this.subIngredienteReceita = this.receitaService.getReceitaIngredientes().subscribe((data)=>{
        this.listaReceitaIngrediente = data;
        data.forEach((elemento)=>{
        if(elemento.idReceita == this.receitaId){
          this.quantidadeIngredientes +=1;
        }
       })
     })
    }

    criaFormCadastro(){
      this.formReceita = this.formBuilder.group({
       nome: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]),
       responsavelReceita: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(200)]),
       estilo: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(80)]),
       volumeReceita: new FormControl('', Validators.required),
       quantidadeIngredientes: [0 , Validators.required],
       ingredientes: this.formBuilder.array([])
      })
    }

  criaFormEdicao(){
    this.formReceita = this.formBuilder.group({
      nome: new FormControl(this.receita.nomeReceita, [Validators.required, Validators.minLength(3), Validators.maxLength(100)]),
      responsavelReceita: new FormControl(this.receita.responsavel, [Validators.required, Validators.minLength(10), Validators.maxLength(200)]),
      estilo: new FormControl(this.receita.estilo, [Validators.required, Validators.minLength(10), Validators.maxLength(80)]),
      volumeReceita: new FormControl(this.receita.volumeReceita, Validators.required),
      quantidadeIngredientes: new FormControl(this.quantidadeIngredientes, Validators.required),
      ingredientes: this.formBuilder.array([ ])
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
            selecionaIngrediente: ['', Validators.required],
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
            if(elemento.id == obj.selecionaIngrediente){
              this.orcamento += obj.quantidade*elemento.valorUnidade;
            }
          })
        })
        return;
    }

    criaIngrediente(): FormGroup {
      return this.formBuilder.group({
        selecionaIngrediente: '',
        quantidade: ''
      });
    }
    carregarIngredienteFormulario(){
      const ingredientes = this.listaReceitaIngrediente.filter((data) => data.idReceita == this.receitaId)

      ingredientes.forEach((elemento)=>{
        this.listaIngredientes.forEach((ingrediente)=>{
          if(elemento.idIngrediente == ingrediente.id){
            this.adicionaIngrediente();
            const control = this.numeroIngredientes;
            const index = control.length-1;

            control.at(index).patchValue({
              selecionaIngrediente: ingrediente.id,
              quantidade: elemento.quantidadeDeIngrediente
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
        responsavel: this.responsavelReceita,
        estilo: this.estilo,
        ultimaAtualizacao: new Date(),
        orcamento: this.orcamento,
        volumeReceita: this.volumeReceita
      }
      await this.receitaService.salvarReceita(Receita).toPromise();
      this.encontraIdReceita();
      this.retornaPaginaReceita();
      return;
    }
     const Receita: any= {
        id: this.receita.id,
        nomeReceita: this.nome,
        responsavel: this.responsavelReceita,
        estilo: this.estilo,
        ultimaAtualizacao: new Date(),
        orcamento: this.orcamento,
        volumeReceita: this.volumeReceita
     }

    this.receitaService.atualizarReceita(Receita).toPromise();
    this.alteraIngredientesReceita()
    this.retornaPaginaReceita();

  }
  encontraIdReceita(){
    this.subReceita = this.receitaService.getReceitas().subscribe((data)=>{
      this.modificaIngredienteReceita(data[data.length-1].id)
    })
  }

  async modificaIngredienteReceita(receitaId : number){

      this.numeroIngredientes.controls.forEach((control)=>{
        const ingredienteReceita : any = {
          idReceita: receitaId,
          idIngrediente: control.value.selecionaIngrediente,
          quantidadeDeIngrediente: control.value.quantidade
        }

      this.receitaService.salvarReceitaIngredientes(ingredienteReceita).toPromise();
      })
    }
    alteraIngredientesReceita(){
      let listaControleIngredientes: any = [];
       this.numeroIngredientes.controls.forEach((control)=>{
        const ingrediente: any = {
           idReceita: parseInt(this.receitaId),
           idIngrediente: parseInt(control.value.selecionaIngrediente),
           quantidadeDeIngrediente: parseInt(control.value.quantidade)
         }
        listaControleIngredientes.push(ingrediente);
        })
        const ingredienteAdicionados = listaControleIngredientes.filter((ingredienteTeste : any) => !this.listaReceitaIngrediente.some(prevIng => prevIng.idIngrediente === ingredienteTeste.idIngrediente));

        const removerIngredientes = this.listaReceitaIngrediente.filter(prevIng => !listaControleIngredientes.some((ingredienteTeste: any) => ingredienteTeste.idIngrediente === prevIng.idIngrediente));

        const alterarIngredientes = listaControleIngredientes.filter((ingrediente:any) => {
          const ingredientesAnterior = this.listaReceitaIngrediente.find(prevIng => prevIng.idIngrediente === ingrediente.idIngrediente);
          return ingredientesAnterior && (ingredientesAnterior.idIngrediente !== ingrediente.idIngrediente || ingredientesAnterior.quantidadeDeIngrediente !== ingrediente.quantidadeDeIngrediente);
        });

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
    const listaFiltrada = this.listaReceitaIngrediente.filter((elemento)=> elemento.idReceita == this.receitaId);
      novaLista.forEach((elemento: any) => {
        listaFiltrada.forEach((data)=>{
          if(elemento.idIngrediente == data.idIngrediente){
            const ingredienteReceita : IReceitaIngrediente = {
              id: data.id,
              idReceita:elemento.idReceita,
              idIngrediente: elemento.idIngrediente,
              quantidadeDeIngrediente: elemento.quantidadeDeIngrediente
            }
            this.receitaService.atualizarReceitaIngrediente(ingredienteReceita).subscribe();
          }
        })
      });
    }

  async removeNovoIngredienteReceita(novaLista: any){
      novaLista.forEach((elemento : any)=>{
        this.receitaService.excluirEspecialReceitaIngrediente(elemento.id).toPromise();
      })
    }

    buscaProducao(){
      this.subProducao = this.producaoService.getProducoes().subscribe((data)=>{
        const producao = data.filter((elemento) => elemento.receitaId == this.receita.id);

        this.excluiReceita(producao);
      });
      this.subProducao.unsubscribe;
    }

   async excluiReceita(producao:any){

    if(producao.length > 0){
      window.alert("Não é possivel excluir uma receita com produção associada!")
      this.router.navigate(['/Receita']);
      return;
    }
    await this.receitaService.excluirReceita(this.receitaId).toPromise().then(()=>{
      this.retornaPaginaReceita()
    });

   }


  retornaPaginaReceita(){

      this.router.navigate(['/receitas']);

  }
  ngOnDestroy(): void {

    this.subReceita.unsubscribe
    this.subIngredientes.unsubscribe;
    this.subIngredienteReceita.unsubscribe;

  }

}
