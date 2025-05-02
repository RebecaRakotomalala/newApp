# newApp

Clé API:   73350ed9ae7972a
Save API Secret: 16105bb9f498033

use _abcd022bd4ccf1e6;


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



------- Voir les Devis rattachers a un appel d'offre et un fournisseur
GET /api/resource/Supplier Quotation?filters=[["supplier","=","Fournisseur1"],["request_for_quotation","=","RFQ-2025-00001"]]

SELECT
    sqi.name AS Nameee,
    sq.name AS supplier_quotation,
    sq.transaction_date,
    sq.status,
    sq.supplier,
    sqi.item_code,
    sqi.qty,
    sqi.rate,
    sqi.amount,
    sqi.request_for_quotation
FROM
    `tabSupplier Quotation` sq
JOIN
    `tabSupplier Quotation Item` sqi ON sq.name = sqi.parent
WHERE
    sqi.request_for_quotation = 'PUR-RFQ-2025-00001'
    AND sq.supplier = 'fournisseur2'
    AND sq.docstatus < 2
ORDER BY
    sq.transaction_date DESC;

*****sq.docstatus < 2 permet d'exclure les documents annulés.

---------- Voir devis where Numéro Devis
SELECT
    sqi.name AS Nameee,
    sq.name AS supplier_quotation,
    sq.transaction_date,
    sq.status,
    sq.supplier,
    sqi.item_code,
    sqi.qty,
    sqi.rate,
    sqi.amount,
    sqi.request_for_quotation
FROM
    `tabSupplier Quotation` sq
JOIN
    `tabSupplier Quotation Item` sqi ON sq.name = sqi.parent
WHERE
    sqi.name = 'fkd4540aq8'
ORDER BY
    sqi.idx


string url = $"http://erpnext.localhost:8000/api/resource/Request%20for%20Quotation?fields=[\"name\",\"transaction_date\",\"status\"]&filters=[['supplier','=', '{fournisseur}']]";