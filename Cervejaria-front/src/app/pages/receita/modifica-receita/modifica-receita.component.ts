import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IIngredientes } from 'src/app/models/ingredientes';
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
  quantidadeIngredientes : any[] = [];



  constructor(private activatedRoute: ActivatedRoute, private receitaService: ReceitasServiceService,
    private formBuilder: FormBuilder, private router: Router,
    private ingredienteService : IngredienteServiceService){}

  ngOnInit(): void {
    this.receitaId = this.activatedRoute.snapshot.paramMap.get('id')
    this.formReceita;
    this.buscaReceita();
    this.buscaIngredientes();
    console.log(this.formReceita);

    //this.oncontagemIngredienteChange();

  }

  buscaIngredientes(){
    this.ingredienteService.getIngredientes().subscribe((data)=>{
      this.listaIngredientes = data;

    })
  }

  verificaTemId(){

    if(this.receitaId == "criar"){
      this.titulo = "Criar Receita"
      //this.defineQuantidadeIngredientes();
      this.criaFormCadastro();

      return;
    }
    this.titulo = "Editar Receita"
    this.criaFormEdicao();
    this.retornaQuantidadeIngredientes();

   }
  //  defineQuantidadeIngredientes(){
  //   this.quantidadeIngredientes.length = this.quantidades;

  //   console.log(this.quantidadeIngredientes);

  //  }
    retornaQuantidadeIngredientes(){
     this.receitaService.getReceitaIngredientes().subscribe((data)=>{
       this.quantidadeIngredientes = data;
     })
    }

    criaFormCadastro(){
      this.formReceita = this.formBuilder.group({
       nome: new FormControl('', Validators.required),
       responsavelReceita: new FormControl('', [Validators.required]),
       estilo: new FormControl('', Validators.required),
       orcamento: new FormControl(this.calculaOrcamento(), Validators.required),
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
      orcamento: new FormControl(this.calculaOrcamento(), Validators.required),
      volumeReceita: new FormControl(this.receita.volumeReceita, Validators.required),
      quantidadeIngredientes: new FormControl( 0 || this.quantidadeIngredientes.length, Validators.required),
      ingredientes: this.formBuilder.array([])
    });
  }

  get quantidades(){
    return this.formReceita.get('quantidadeIngredientes')?.value;
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
  get orcamento(){
    return this.formReceita.get('orcamento')?.value;
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
  }
    buscaReceita(){
      if(this.receitaId == "criar"){
        this.verificaTemId();
        return;
     }
     this.receitaService.getReceita(parseFloat(this.receitaId)).subscribe((data)=>{
      this.receita = data;
      this.verificaTemId();
     })
    }
    calculaOrcamento(){}

    modificaReceita(){}
  // async modificaReceita(){
  //   if(this.receitaId === "criar"){
  //     const Receita: any= {
  //       nomeReceita: this.nome,
  //       responsavelReceita: this.responsavelReceita,
  //       estilo: this.estacao,
  //       ultima_atualizacao: new Date(),
  //       orcamento: this.calculaOrcamento(),
  //       lancamento: this.lancamento,
  //       volumeReceita:
  //     }
  //     "nomeReceita": "Pilsen",
  //     "responsavelReceita": "Murilo",
  //     "estilo": "American Lager",
  //     "ultima_atualizacao": "2023/04/20",
  //     "orcamento": 9500,
  //     "volumeReceita": 2000


  //     await this.ReceitaService.criarReceita(Receita).toPromise();
  //     this.retornaPaginaReceita();
  //     return;
  //   }
  //   const Receita: any= {
  //     id: this.Receita.id,
  //     nomeReceita: this.nomeReceita,
  //     responsavelReceita: this.responsavelReceita,
  //     estacao: this.estacao,
  //     marca: this.marca,
  //     orcamento: parseFloat(this.orcamento),
  //     lancamento: this.lancamento,

  //   }

  //   this.ReceitaService.atualizarReceita(Receita).subscribe();
  //   this.retornaPaginaReceita();

  // }
  // retornaModelos(){

  //   this.modeloService.getModelos().subscribe((data) =>{
  //     this.listaModelos=data;

  //   })

  // }

  // retornaTotalModelos(id: number){
  //   let numeroModelos = 0;

  //   this.listaModelos.map((date) =>{

  //     if(date.ReceitaId == id){
  //       numeroModelos+=1;
  //     }
  //  })
  //  return numeroModelos;
  // }

  // excluiReceita(){

  //    if(this.retornaTotalModelos(this.ReceitaId) === 0){
  //     this.ReceitaService.excluirReceita(this.ReceitaId).subscribe();
  //     this.router.navigate(['/Receita']);
  //     return;
  //   }
  //   alert("Impossível excluir coleções com modelos associados!")
  //   this.retornaPaginaReceita();
  // }

  // retornaPaginaReceita(){

  //     this.router.navigate(['/Receita']);

  // }

}
