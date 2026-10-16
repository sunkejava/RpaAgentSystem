-- 在数据库迁移完成后执行。所有租户表默认拒绝未设置 app.tenant_id 的连接。
CREATE TABLE IF NOT EXISTS "TenantMembers" (
 "Id" uuid PRIMARY KEY,
 "TenantId" uuid NOT NULL REFERENCES "Tenants"("Id"),
 "Subject" text NOT NULL,
 "Role" text NOT NULL,
 "Enabled" boolean NOT NULL DEFAULT true,
 "CreatedAt" timestamptz NOT NULL DEFAULT now(),
 UNIQUE ("TenantId","Subject")
);
ALTER TABLE "TenantMembers" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "TenantMembers" FORCE ROW LEVEL SECURITY;
CREATE POLICY tenant_boundary ON "TenantMembers"
 USING ("TenantId" = NULLIF(current_setting('app.tenant_id', true), '')::uuid)
 WITH CHECK ("TenantId" = NULLIF(current_setting('app.tenant_id', true), '')::uuid);
