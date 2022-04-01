using Tarefas.db;

bool sair = false;
while (!sair)
{
    string opcao = UI.SelecionaOpcaoEmMenu();

    switch (opcao)
    {
        case "L": ListarTodasAsTarefas(); break;
        case "P": ListarTarefasPendentes(); break;
        case "I": ListarTarefasPorId(); break;
        case "D": ListarTarefasPorDescricao(); break;
        case "N": IncluirNovaTarefa(); break;
        case "A": AlterarDescricaoDaTarefa(); break;
        case "C": ConcluirTarefa(); break;
        case "E": ExcluirTarefa(); break;

        case "S":
            sair = true;
            break;

        default:
            UI.ExibeErro("\nOpção não reconhecida.");
            break;
    }

    Console.Write("\nPressione uma tecla para continuar...");
    Console.ReadKey();
}

void ListarTodasAsTarefas()
{
    UI.ExibeDestaque("\n-- Listar todas as Tarefas ---");

    using (var db = new tarefasContext())
    {
        var tarefas = db.Tarefa.ToList<Tarefa>();

        Console.WriteLine($"{tarefas.Count()} tarefas encontradas...");

        foreach (var tarefa in tarefas)
        {
            Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
        }
    }
}

void ListarTarefasPendentes()
{
    UI.ExibeDestaque("\n-- Listar Tarefas Pendentes ---");
    
    using (var db = new tarefasContext())
    {
        var tarefas = db.Tarefa
        .OrderByDescending(t => t.Id)
        .Where(t => !t.Concluida)
        .ToList<Tarefa>();

        Console.WriteLine($"{tarefas.Count()} tarefas encontradas...");

        foreach (var tarefa in tarefas)
        {
            Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
        }
    }
}

void ListarTarefasPorDescricao()
{
    UI.ExibeDestaque("\n-- Listar Tarefas por Descrição ---");
    string descricao = UI.SelecionaDescricao();
    
    using (var db = new tarefasContext())
    {
        var tarefas = db.Tarefa
        .Where(t => t.Descricao.Contains(descricao))
        .ToList<Tarefa>();

        Console.WriteLine($"{tarefas.Count()} tarefas encontradas...");

        foreach (var tarefa in tarefas)
        {
            Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
        }
    }
}

void ListarTarefasPorId()
{
    UI.ExibeDestaque("\n-- Listar Tarefas por Id ---");
    int id = UI.SelecionaId();
    
    using (var db = new tarefasContext())
    {
        var tarefa = db.Tarefa.Find(id);

        if (tarefa == null)
        {
            Console.WriteLine("Tarefa não encontrada...");
            return;
        }
        
        Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
    }
}

void IncluirNovaTarefa()
{
    UI.ExibeDestaque("\n-- Incluir Nova Tarefa ---");
    string descricao = UI.SelecionaDescricao();
    
    if (String.IsNullOrEmpty(descricao))
    {
        UI.ExibeErro("Não é possível incluir tarefa sem descrição...");
        return;
    }

    using (var db = new tarefasContext())
    {
        var tarefa = new Tarefa
        {
            Descricao = descricao,
        };

        db.Tarefa.Add(tarefa);
        db.SaveChanges();

        Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
    }
}

void AlterarDescricaoDaTarefa()
{
    UI.ExibeDestaque("\n-- Alterar Descrição da Tarefa ---");
    int id = UI.SelecionaId();
    string descricao = UI.SelecionaDescricao();
    
    if (String.IsNullOrEmpty(descricao))
    {
        UI.ExibeErro("Não é permitido criar uma tarefa sem descrição...");
        return;
    }

    using (var db = new tarefasContext())
    {
        var tarefa = db.Tarefa.Find(id);

        if (tarefa == null)
        {
            Console.WriteLine("Tarefa não encontrada...");
            return;
        }
        
        tarefa.Descricao = descricao;
        db.SaveChanges();

        Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
    }
}

void ConcluirTarefa()
{
    UI.ExibeDestaque("\n-- Concluir Tarefa ---");
    int id = UI.SelecionaId();
    
    using (var db = new tarefasContext())
    {
        var tarefa = db.Tarefa.Find(id);

        if (tarefa == null)
        {
            Console.WriteLine("Tarefa não encontrada...");
            return;
        }

        if (tarefa.Concluida)
        {
            UI.ExibeErro("Tarefa já concluída...");
            return;
        }
        
        tarefa.Concluida = true;
        db.SaveChanges();

        Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
    }
}

void ExcluirTarefa()
{
    UI.ExibeDestaque("\n-- Excluir Tarefa ---");
    int id = UI.SelecionaId();
    
    using (var db = new tarefasContext())
    {
        var tarefa = db.Tarefa.Find(id);

        if (tarefa == null)
        {
            Console.WriteLine("Tarefa não encontrada...");
            return;
        }

        db.Tarefa.Remove(tarefa);
        db.SaveChanges();
        
        Console.WriteLine($"[{(tarefa.Concluida ? "X" : " ")}] #{tarefa.Id}: {tarefa.Descricao}");
    }
}
