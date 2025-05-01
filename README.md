# newApp

Clé API:   73350ed9ae7972a
Save API Secret: 16105bb9f498033


------- Voir appel d'ouffre where fournisseur
SELECT 
    rqf.name AS numero_devis,
    s.supplier AS Supplier,
    rqf.transaction_date AS date_transaction,
    rqf.schedule_date AS date_livraison_prevue,
    rqf.status,
    rqf.message_for_supplier AS message
FROM `tabRequest for Quotation` AS rqf
JOIN `tabRequest for Quotation Supplier` AS s
  ON rqf.name = s.parent
WHERE s.supplier = 'fournisseur1';

