# newApp

Clé API:   73350ed9ae7972a
Save API Secret: 16105bb9f498033
Cookies : bdb4407e2398d6e52ded2c560458a594a89976aa5e3ed8790913246d

use _abcd022bd4ccf1e6;


wine /opt/looping-mcd/Looping.exe


stackoverflow, MDN Web Docs, w3schools,


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


SELECT
            po.name AS purchase_order,
            po.transaction_date,
            po.schedule_date,
            po.status,
            po.supplier,
            poi.item_code,
            poi.qty,
            poi.rate,
            poi.amount
        FROM
            `tabPurchase Order` AS po
        JOIN
            `tabPurchase Order Item` AS poi ON po.name = poi.parent
        WHERE
            po.supplier = 'fournisseur3'
        AND
            po.status = 'To Receive'
        ORDER BY
            po.transaction_date DESC






| Colonne              | Description                                                               |
| -------------------- | ------------------------------------------------------------------------- |
| `grand_total`        | Montant total de la facture (TTC — y compris taxes, frais, remises, etc.) |
| `outstanding_amount` | **Reste à payer** (le montant non encore payé sur cette facture)          |
| `paid_amount`        | Montant déjà payé (utile si paiement partiel)                             |
| `total`              | Montant total **hors taxes et remises**                                   |

SELECT
    pi.name AS invoice_name,
    pi.supplier,
    pi.grand_total,
    pi.outstanding_amount,
    pe.name AS payment_name,
    pe.party_type,
    pe.mode_of_payment,
    pe.paid_amount,
    pe.posting_date AS payment_date
FROM `tabPurchase Invoice` pi
LEFT JOIN `tabPayment Entry` pe
    ON pe.party = pi.supplier AND pe.party_type = 'Supplier'
WHERE pe.docstatus = 1 OR pe.name IS NULL
ORDER BY pi.posting_date DESC, pe.posting_date DESC;




| Statut (`status`)      | Signification                                                             |
| ---------------------- | ------------------------------------------------------------------------- |
| **Draft**              | La facture est encore à l'état de brouillon (non soumise).                |
| **Submitted**          | Elle est soumise mais son statut précis dépend de son paiement.           |
| **Unpaid**             | Facture soumise mais aucun paiement encore effectué.                      |
| **Partly Paid**        | Une partie de la facture a été payée.                                     |
| **Paid**               | La facture a été complètement payée.                                      |
| **Overdue**            | La date d’échéance est dépassée et la facture n’est pas totalement payée. |
| **Cancelled**          | La facture a été annulée.                                                 |
| **Credit Note Issued** | Une note de crédit a été émise contre cette facture.                      |

✅ Représentent des factures impayées :
| Statut          | Description                                                                    |
| --------------- | ------------------------------------------------------------------------------ |
| **Unpaid**      | Aucune somme n’a encore été payée.                                             |
| **Partly Paid** | Une partie de la facture a été payée, mais il reste un montant à régler.       |
| **Overdue**     | La facture n’est pas totalement réglée **et la date d’échéance est dépassée**. |



Dans ERPNext, la table du grand livre (general ledger) s'appelle :
▶️ tabGL Entry   

✅ 1. Requête SQL pour identifier le Doctype d'une table
SELECT name 
FROM `tabDocType` 
WHERE `db_table` = 'tabPurchase Invoice';




SELECT 
    po.supplier,
    po.name AS purchase_order,
    pi.name AS purchase_invoice,
    po.per_received,
    po.grand_total,
    po.status AS status_po,
    pi.status AS status_pi,
    pi.outstanding_amount,
    pii.item_name,
    pii.qty,
    pii.rate,
    pii.amount
FROM 
    tabPurchase Order po
JOIN 
    tabPurchase Invoice Item pii ON pii.purchase_order = po.name
JOIN 
    tabPurchase Invoice pi ON pi.name = pii.parent
GROUP BY 
    po.name, pi.name, po.per_received, po.grand_total,pi.status,po.supplier,
    pii.rate, pii.amount, pii.qty, pii.item_name,
     po.status, pi.outstanding_amount;