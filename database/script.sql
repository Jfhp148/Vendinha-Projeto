CREATE TABLE IF NOT EXISTS "Clientes" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Clientes" PRIMARY KEY AUTOINCREMENT,
    "Nome" TEXT NOT NULL,
    "Cpf" TEXT NOT NULL,
    "DataNascimento" TEXT NOT NULL,
    "Email" TEXT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Clientes_Cpf" ON "Clientes" ("Cpf");

CREATE TABLE IF NOT EXISTS "Dividas" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Dividas" PRIMARY KEY AUTOINCREMENT,
    "Valor" TEXT NOT NULL,
    "Paga" INTEGER NOT NULL,
    "DataCriacao" TEXT NOT NULL,
    "DataPagamento" TEXT NULL,
    "ClienteId" INTEGER NOT NULL,
    CONSTRAINT "FK_Dividas_Clientes_ClienteId" FOREIGN KEY ("ClienteId") REFERENCES "Clientes" ("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_Dividas_ClienteId" ON "Dividas" ("ClienteId");
