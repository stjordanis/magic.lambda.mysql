﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using magic.node;
using magic.data.common;
using magic.signals.contracts;

namespace magic.lambda.mysql
{
    /// <summary>
    /// [mysql.execute] slot for executing a non query SQL command.
    /// </summary>
    [Slot(Name = "mysql.execute")]
    public class Execute : ISlot, ISlotAsync
    {
        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            Executor.Execute(
                input, 
                signaler.Peek<MySqlConnection>("mysql.connect"),
                signaler.Peek<Transaction>("mysql.transaction"),
                (cmd) =>
            {
                input.Value = cmd.ExecuteNonQuery();
            });
        }

        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        /// <returns>An awaitable task.</returns>
		public async Task SignalAsync(ISignaler signaler, Node input)
        {
            await Executor.ExecuteAsync(
                input, 
                signaler.Peek<MySqlConnection>("mysql.connect"),
                signaler.Peek<Transaction>("mysql.transaction"),
                async (cmd) =>
            {
                input.Value = await cmd.ExecuteNonQueryAsync();
            });
        }
    }
}
