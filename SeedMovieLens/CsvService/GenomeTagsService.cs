using CsvHelper;
using CsvHelper.Configuration;
using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedMovieLens.CsvService
{
    public class GenomeTagsService
    {
        private readonly DbContextOptionsBuilder<ApplicationDbContext> OptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Ler o arquivo CSV e dividir a carga de acordo com o número de Threads (De acordo com o volume de dados, aplicar um número maior de Threads para popular a tabela mais rápido)
        /// </summary>
        /// <param name="location">Caminho do arquivo CSV</param>
        /// <param name="connectionString">Connection String para acessar o banco de dados</param>
        /// <param name="threads">Número de Threads a serem utilizados</param>
        public void ReadCsvFile(string location, string connectionString, int threads)
        {
            OptionsBuilder.UseNpgsql(connectionString);
            try
            {
                int countRecords = 0;
                using (var reader = new StreamReader(location, Encoding.UTF8))
                {
                    while (reader.ReadLine() != null)
                    {
                        countRecords++;
                    }
                    countRecords--;
                }
                Logger.Info($"Inserting {countRecords} genomeTags using {threads} threads");

                //Algoritmo para realizar a divisão de tarefas entre as Threads
                int divisao = countRecords / threads;
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < threads; i++)
                {
                    int skip = divisao * i;
                    int take = countRecords - divisao * i;
                    if (i == threads - 1)
                    {
                        tasks.Add(Task.Run(() => StartThread(location, new ApplicationDbContext(OptionsBuilder.Options), skip, take)));
                    }
                    else
                    {
                        tasks.Add(Task.Run(() => StartThread(location, new ApplicationDbContext(OptionsBuilder.Options), skip, divisao)));
                    }
                }


                Task.WaitAll(tasks.ToArray());
                Logger.Info("GenomeTags inserted");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ler o arquivo CSV de acordo com a linha que deve começar e a quantidade de linhas que devem ser lidas
        /// </summary>
        /// <param name="location">Caminho do arquivo CSV</param>
        /// <param name="context">DbContext do banco de dados</param>
        /// <param name="skip">Linha de início</param>
        /// <param name="take">Quantidade de linhas</param>
        private void StartThread(string location, DbContext context, int skip, int take)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                using var reader = new StreamReader(location, Encoding.UTF8);

                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Configuration.RegisterClassMap<GenomeTagMap>();
                int count = 0;
                foreach (var r in csv.GetRecords<GenomeTags>().Skip(skip).Take(take))
                {
                    context.Add(r);
                    //A cada 1000 linhas, salva no banco de dados, em vez de salvar uma por uma
                    if (count == 1000)
                    {
                        context.SaveChanges();
                        DetachEntries(context);
                        count = 0;
                    }
                    count++;
                }
                context.SaveChanges();
                DetachEntries(context);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        /// <summary>
        /// Função para fazer com que o DbContext pare de manter o Tracking das Entries que vão sendo adicionadas
        /// </summary>
        /// <param name="context"></param>
        private void DetachEntries(DbContext context)
        {
            foreach (var item in context.ChangeTracker.Entries<GenomeTags>())
            {
                item.State = EntityState.Detached;
            }
        }
    }

    /// <summary>
    /// Classe para mapear os campos do arquivo CSV com a Classe de destino
    /// </summary>
    public sealed class GenomeTagMap : ClassMap<GenomeTags>
    {
        public GenomeTagMap()
        {
            Map(x => x.TagId).Name("tagId");
            Map(x => x.Tag).Name("tag");
        }
    }
}
