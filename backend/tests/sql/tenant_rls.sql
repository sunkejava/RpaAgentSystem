-- CI SQL checks for RLS behavior.
CREATE ROLE rpa_ci_reader NOLOGIN;
GRANT USAGE ON SCHEMA public TO rpa_ci_reader;
GRANT SELECT ON "TenantMembers" TO rpa_ci_reader;
INSERT INTO "Tenants" ("Id","Name") VALUES ('11111111-1111-1111-1111-111111111111','A'),('22222222-2222-2222-2222-222222222222','B');
INSERT INTO "TenantMembers" ("Id","TenantId","Subject","Role") VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa','11111111-1111-1111-1111-111111111111','a','tenant-admin'),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb','22222222-2222-2222-2222-222222222222','b','tenant-admin');
SET ROLE rpa_ci_reader;
DO $$ BEGIN IF (SELECT count(*) FROM "TenantMembers") <> 0 THEN RAISE EXCEPTION 'unscoped rows'; END IF; END $$;
BEGIN;
SELECT set_config('app.tenant_id','11111111-1111-1111-1111-111111111111',true);
DO $$ BEGIN IF (SELECT count(*) FROM "TenantMembers") <> 1 THEN RAISE EXCEPTION 'scoped rows'; END IF; END $$;
COMMIT;
DO $$ BEGIN IF (SELECT count(*) FROM "TenantMembers") <> 0 THEN RAISE EXCEPTION 'context leaked'; END IF; END $$;
RESET ROLE;
